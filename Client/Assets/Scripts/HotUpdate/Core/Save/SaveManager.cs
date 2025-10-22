using System;
using System.Collections.Generic;

namespace GameFramework.Core 
{
    [GameModule(GameModulePriority.SaveManager)]
    public class SaveManager : IGameModule_SyncInit
    {
        private const string SlotKey = "SLOT";
        private const string DataKey = "SAVE_DATA_{0}";
        private const string FilePath = "saveData";
        private const int SlotCount = 20;

        private List<DataModel> slots;

        public void Init()
        {
            LoadSlots();
        }

        public void Save(int index) 
        {
            string key = GetKey(index);

            SaveWriter writer = new SaveWriter();
            Game.Gameplay.SaveGame(writer);
            SaveData data = writer.GetData();

            ES3.Save(key, data, FilePath);
            ES3.Save(SlotKey, slots, FilePath);
            MDebug.Log("Save!");
        }

        public void Load(int index) 
        {
            string key = GetKey(index);

            SaveData data;
            if (ES3.FileExists(FilePath))
            {
                data = ES3.Load<SaveData>(key, FilePath);
            }
            else
            {
                data = new SaveData();
            }
            SaveReader reader = new SaveReader();
            reader.SetData(data);

            Game.Gameplay.LoadGame(reader);
            MDebug.Log("Load!");
        }

        private string GetKey(int index) 
        {
            string key = string.Format(DataKey, index);
            return key;
        }

        private void LoadSlots() 
        {
            slots = new List<DataModel>(SlotCount);
            for (int i = 0; i < SlotCount; i++)
            {
                DataModel slot = new DataModel();
                slot.SetValue("index", i);
                slots.Add(slot);
            }

            if (ES3.FileExists(FilePath))
            {
                var data = ES3.Load<List<DataModel>>(SlotKey, FilePath);
                for (int i = 0; i < data.Count; i++)
                {
                    slots[i] = data[i];
                }
            }
        }
    }
}