using System;
using System.Collections.Generic;

namespace GameFramework.Core
{
    [GameSystem]
    public class GameSaveSystem : IGameSystem, IInit
    {
        private const string SaveFileName = "saveData";
        private const string SaveKeyFormat = "SAVE_DATA_{0}";

        private const string SummaryFileName = "saveSummaryData";
        private const string SummaryKey = "SUMMARY";

        private List<ISavableGameModule> savables;
        private IGameSaveSummary saveSummary;
        private IGameSaveStorage storage;

        void IInit.Init()
        {
            savables = new List<ISavableGameModule>();
            // 默认使用 ES3 后端;通过 SetStorage 可替换为其它持久化实现而无需改动本系统
            storage = new ES3GameSaveStorage();
        }

        /// <summary>
        /// 替换持久化后端。便于切换存储方案(自定义二进制、云存档)或在测试中注入内存实现。
        /// </summary>
        public void SetStorage(IGameSaveStorage customStorage)
        {
            if (customStorage != null)
            {
                storage = customStorage;
            }
        }

        public void Register(ISavableGameModule savable)
        {
            savables.Add(savable);
        }

        public void RegisterSaveSummaryHandler(IGameSaveSummary handler)
        {
            saveSummary = handler;
        }

        public void LoadSaveSummary()
        {
            MDebug.Log("LoadSaveSummary");
            IGameSaveData data = storage.Load(SummaryFileName, SummaryKey);
            saveSummary?.Load(data);
            MDebug.Log("LoadSaveSummary End");
        }

        public void SaveGame(int index)
        {
            try
            {
                MDebug.Log("Save Game!");
                IGameSaveWriter writer = new GameSaveWriter();
                foreach (var savable in savables)
                {
                    writer.MoveNext(savable.GetType().FullName);
                    savable.OnSaveGame(writer);
                }

                IGameSaveData data = writer.GetData();
                storage.Save(SaveFileName, GetSaveKey(index), data);

                IGameSaveData summaryData = saveSummary.Save(index);
                storage.Save(SummaryFileName, SummaryKey, summaryData);

                MDebug.Log("Save Game Finish!");
            }
            catch (Exception e)
            {
                MDebug.Error(e);
            }
        }

        public void LoadGame(int index)
        {
            try
            {
                MDebug.Log("Load Game, index = ", index);
                IGameSaveReader reader = new GameSaveReader();

                IGameSaveData data = storage.Load(SaveFileName, GetSaveKey(index));
                reader.SetData(data);

                foreach (var savable in savables)
                {
                    reader.MoveNext(savable.GetType().FullName);
                    savable.OnLoadGame(reader);
                }

                MDebug.Log("Load Game Finish!");
            }
            catch (Exception e)
            {
                MDebug.Error(e);
            }
        }

        public void DeleteSaveData(int index)
        {
            try
            {
                MDebug.Log("Delete Save Data, index = ", index);

                // 删除该槽位的键而非写入空数据,避免存档文件残留无效内容;读取缺失键已由 storage 兜底
                storage.Delete(SaveFileName, GetSaveKey(index));

                // summary 是所有槽位打包的单一 blob,需让其重置该槽位后整体重新落盘
                IGameSaveData summaryData = saveSummary.Delete(index);
                storage.Save(SummaryFileName, SummaryKey, summaryData);

                MDebug.Log("Delete Save Data Finish!");
            }
            catch (Exception e)
            {
                MDebug.Error(e);
            }
        }

        private string GetSaveKey(int index)
        {
            return string.Format(SaveKeyFormat, index);
        }
    }
}
