using System;
using System.Collections.Generic;

namespace GameFramework.Core 
{
    public struct SaveSlotsEventArgs : IGameEventArgs 
    {
        public DataModel slot;
    }

    [GameModule]
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

            SaveSlots(index);

            MDebug.Log("Save!");
        }

        public void Load(int index) 
        {
            SaveData data;
            if (index > 0 && index < SlotCount) 
            {          
                if (ES3.FileExists(FilePath))
                {
                    string key = GetKey(index);
                    data = ES3.Load<SaveData>(key, FilePath);
                }
                else
                {
                    data = new SaveData();
                }
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

        public bool IsEmptySlot(int index) 
        {
            DataModel slot = GetSlot(index);
            if (slot == null) 
            { 
                return false; 
            }
            return slot.GetBoolValue("has_data");
        }

        public bool HasAnySaveData() 
        {
            for (int i = 0; i < slots.Count; i++)
            {
                DataModel slot = slots[i];
                if (slot.GetBoolValue("has_data")) 
                {
                    return true;
                }
            }
            return false;
        }

        public int GetSaveSceneId(int index) 
        {
            if (index == 0) 
            {
                return 1001;
            }

            DataModel slot = GetSlot(index);
            if (slot == null)
            {
                return 1001;
            }
            return slot.GetIntValue("scene_id");
        }

        public void RemoveSaveData(int index) 
        {
            string key = GetKey(index);

            SaveWriter writer = new SaveWriter();
            SaveData data = writer.GetData();

            ES3.Save(key, data, FilePath);

            DataModel slot = GetSlot(index);
            slot.SetValue("has_data", false);

            ES3.Save(SlotKey, slots, FilePath);
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

        private void SaveSlots(int index) 
        {
            DataModel slot = GetSlot(index);

            Game.Event.Publish(new SaveSlotsEventArgs() { slot = slot });

            slot.SetValue("has_data", true);

            ES3.Save(SlotKey, slots, FilePath);
        }

        private DataModel GetSlot(int index) 
        {
            if (index < 0 || index >= slots.Count) 
            {
                return null;
            }

            DataModel slot = slots[index]; ;
            return slot;
        }
    }
}