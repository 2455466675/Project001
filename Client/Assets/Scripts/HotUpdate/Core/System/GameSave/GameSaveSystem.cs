using System;
using System.Collections.Generic;

namespace GameFramework.Core
{
    [GameSystem]
    public class GameSaveSystem : IGameSystem, IInit
    {
        // 一档一文件
        private const string SlotFileFormat = "save_slot_{0}";
        private const string SlotDataKey = "DATA";

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
                storage.Save(GetSlotFileName(index), SlotDataKey, data);

                saveSummary.Save(index);
                PersistSummary();

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

                IGameSaveData data = storage.Load(GetSlotFileName(index), SlotDataKey);
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

                storage.Delete(GetSlotFileName(index), SlotDataKey);

                saveSummary.Delete(index);
                PersistSummary();

                MDebug.Log("Delete Save Data Finish!");
            }
            catch (Exception e)
            {
                MDebug.Error(e);
            }
        }

        /// <summary>
        /// 导出某档位的原始字节。档位为空时返回 null。
        /// </summary>
        public byte[] ExportSlotRaw(int index)
        {
            return storage.ReadRaw(GetSlotFileName(index));
        }

        /// <summary>
        /// 导入存档数据
        /// </summary>
        public void ImportSlotRaw(int index, byte[] blob)
        {
            storage.WriteRaw(GetSlotFileName(index), blob);
        }

        /// <summary>
        /// json转摘要数据
        /// </summary>
        /// <param name="index">存档index</param>
        /// <param name="json"></param>
        public void JsonToSummary(int index, string json)
        {
            saveSummary?.JsonToSummary(index, json);
        }

        /// <summary>
        /// 摘要数据转json
        /// </summary>
        /// <param name="index">存档index</param>
        /// <returns></returns>
        public string SummaryToJson(int index)
        {
            return saveSummary?.SummaryToJson(index);
        }

        /// <summary>
        /// 保存摘要数据
        /// </summary>
        public void PersistSummary()
        {
            if (saveSummary != null)
            {
                storage.Save(SummaryFileName, SummaryKey, saveSummary.Capture());
            }
        }

        private string GetSlotFileName(int index)
        {
            return string.Format(SlotFileFormat, index);
        }
    }
}
