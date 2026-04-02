using System.Collections.Generic;

namespace GameFramework.Core
{
    [GameSystem]
    public class GameSaveSystem : IGameSystem, IInit
    {
        private const string DataKey = "SAVE_DATA_{0}";
        private const string FilePath = "saveData";

        private List<IGameSavable> savables;

        void IInit.Init()
        {
            savables = new List<IGameSavable>();
        }

        public void Register(IGameSavable savable)
        {
            savables.Add(savable);
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
                Save(index, data);

                MDebug.Log("Save Game Finish!");
            }
            catch (System.Exception e)
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

                IGameSaveData data = Load(index);
                reader.SetData(data);

                foreach (var savable in savables)
                {
                    reader.MoveNext(savable.GetType().FullName);
                    savable.OnLoadGame(reader);
                }

                MDebug.Log("Load Game Finish!");
            }
            catch (System.Exception e)
            {
                MDebug.Error(e);
            }
        }

        private string GetSaveDataKey(int index)
        {
            string key = string.Format(DataKey, index);
            return key;
        }

        private void Save(int index, IGameSaveData data)
        {
            string key = GetSaveDataKey(index);
            ES3.Save(key, data as GameSaveData, FilePath);
        }

        private IGameSaveData Load(int index)
        {
            IGameSaveData data = null;
            if (ES3.FileExists(FilePath))
            {
                string key = GetSaveDataKey(index);
                data = ES3.Load<GameSaveData>(key, FilePath);
            }
            data ??= new GameSaveData();
            return data;
        }
    }
}


