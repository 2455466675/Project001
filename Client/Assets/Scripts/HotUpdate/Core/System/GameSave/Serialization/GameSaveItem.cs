using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    [Serializable]
    public class GameSaveItem : IGameSaveItem
    {
        [Serializable]
        private class SavableEntry
        {
            public int typeId;
            public GameSaveItem item;
        }

        [Serializable]
        private class SavableDict<T>
        {
            public int Count => Math.Min(keys.Count, values.Count);

            public List<T> keys;
            public List<SavableEntry> values;

            public SavableDict(int Count)
            {
                keys = new List<T>(Count);
                values = new List<SavableEntry>(Count);
            }
        }

        [Serializable]
        private class SerializableItem<T>
        {
            public int Count => Math.Min(keys.Count, values.Count);

            public List<T> keys = new List<T>();
            public List<object> values = new List<object>();
        }

        [SerializeField]
        private Dictionary<string, int> intValues;
        [SerializeField]
        private Dictionary<string, long> longValues;
        [SerializeField]
        private Dictionary<string, double> doubleValues;
        [SerializeField]
        private Dictionary<string, string> stringValues;
        [SerializeField]
        private Dictionary<string, SavableEntry> savableDatas;
        [SerializeField]
        private Dictionary<string, List<SavableEntry>> savableDataLists;
        [SerializeField]
        private Dictionary<string, SavableDict<int>> savableDictIntKey;
        [SerializeField]
        private Dictionary<string, SavableDict<string>> savableDictStrKey;

        public GameSaveItem()
        {
            intValues = new Dictionary<string, int>();
            longValues = new Dictionary<string, long>();
            doubleValues = new Dictionary<string, double>();
            stringValues = new Dictionary<string, string>();
            savableDatas = new Dictionary<string, SavableEntry>();
            savableDataLists = new Dictionary<string, List<SavableEntry>>();
            savableDictIntKey = new Dictionary<string, SavableDict<int>>();
            savableDictStrKey = new Dictionary<string, SavableDict<string>>();
        }

        public void Write(string key, int value)
        {
            intValues[key] = value;
        }

        public void Write(string key, long value)
        {
            longValues[key] = value;
        }

        public void Write(string key, bool value)
        {
            intValues[key] = value ? 1 : 0;
        }

        public void Write(string key, float value)
        {
            doubleValues[key] = value;
        }

        public void Write(string key, double value)
        {
            doubleValues[key] = value;
        }

        public void Write(string key, string value)
        {
            stringValues[key] = value;
        }

        public void Write(string key, ISavableData value)
        {
            if (value == null)
            {
                return;
            }
            savableDatas[key] = CreateEntry(SaveTypeRegistry.GetTypeId(value), value);
        }

        public void Write<T>(string key, List<T> value) where T : class, ISavableData
        {
            if (value == null || value.Count == 0)
            {
                return;
            }
            int typeId = SaveTypeRegistry.GetTypeId(value[0]);
            List<SavableEntry> datas = new List<SavableEntry>(value.Count);
            foreach (var item in value)
            {                                
                SavableEntry entry = CreateEntry(typeId, item);
                datas.Add(entry);
            }
            savableDataLists[key] = datas;
        }

        public void Write<T>(string key, Dictionary<int, T> value) where T : class, ISavableData
        {
            if (value == null || value.Count == 0)
            {
                return;
            }
            
            SavableDict<int> dic = new SavableDict<int>(value.Count);
            int index = 0;
            foreach (var item in value)
            {
                dic.keys[index] = item.Key;
                dic.values[index] = CreateEntry(SaveTypeRegistry.GetTypeId(item.Value), item.Value);
                index++;
            }
            savableDictIntKey[key] = dic;
        }

        public void Write<T>(string key, Dictionary<string, T> value) where T : class, ISavableData
        {
            if (value == null || value.Count == 0)
            {
                return;
            }

            SavableDict<string> dic = new SavableDict<string>(value.Count);
            int index = 0;
            foreach (var item in value)
            {
                dic.keys[index] = item.Key;
                dic.values[index] = CreateEntry(SaveTypeRegistry.GetTypeId(item.Value), item.Value);
                index++;
            }
            savableDictStrKey[key] = dic;
        }

        public int ReadInt(string key)
        {
            if (intValues.ContainsKey(key))
            {
                return intValues[key];
            }
            else
            {
                return 0;
            }
        }

        public long ReadLong(string key)
        {
            if (longValues.ContainsKey(key))
            {
                return longValues[key];
            }
            else
            {
                return 0;
            }
        }

        public bool ReadBool(string key)
        {
            if (intValues.ContainsKey(key))
            {
                return intValues[key] == 1;
            }
            else
            {
                return false;
            }
        }

        public float ReadFloat(string key)
        {
            if (doubleValues.ContainsKey(key))
            {
                return (float)doubleValues[key];
            }
            else
            {
                return 0f;
            }
        }

        public double ReadDouble(string key)
        {
            if (doubleValues.ContainsKey(key))
            {
                return doubleValues[key];
            }
            else
            {
                return 0f;
            }
        }

        public string ReadString(string key)
        {
            if (stringValues.ContainsKey(key))
            {
                return stringValues[key];
            }
            else
            {
                return string.Empty;
            }
        }

        public T ReadData<T>(string key) where T : class, ISavableData
        {
            if (!savableDatas.TryGetValue(key, out var entry))
            {
                return default;
            }
            ISavableData data = SaveTypeRegistry.Create(entry.typeId);  // 按 id new,不反射类名
            data.OnRead(entry.item);
            if (data is T typed)
            {
                return typed;
            }
            MDebug.Error("ReadData 类型不匹配, key = ", key, " typeId = ", entry.typeId);
            return default;
        }

        public List<T> ReadList<T>(string key) where T : class, ISavableData
        {
            if (savableDataLists.TryGetValue(key, out var value))
            {
                List<T> result = new List<T>(value.Count);
                foreach (var entry in value)
                {
                    ISavableData data = SaveTypeRegistry.Create(entry.typeId);
                    data.OnRead(entry.item);
                    result.Add(data as T);
                }
                return result;
            }
            else
            {
                 return new List<T>();
            }
        }

        public Dictionary<int, T> ReadDicWithIntKey<T>(string key) where T : class, ISavableData
        {
            Dictionary<int, T> result = new Dictionary<int, T>();
            if (savableDictIntKey.TryGetValue(key, out var dic))
            {
                for (int i = 0; i < dic.Count; i++)
                {
                    int k = dic.keys[i];
                    var entry = dic.values[i];
                    ISavableData data = SaveTypeRegistry.Create(entry.typeId);
                    data.OnRead(entry.item);
                    result[k] = data as T;
                }
            }
            return result;
        }

        public Dictionary<string, T> ReadDicWithStringKey<T>(string key) where T : class, ISavableData
        {
            Dictionary<string, T> result = new Dictionary<string, T>();
            if (savableDictStrKey.TryGetValue(key, out var dic))
            {
                for (int i = 0; i < dic.Count; i++)
                {
                    string k = dic.keys[i];
                    var entry = dic.values[i];
                    ISavableData data = SaveTypeRegistry.Create(entry.typeId);
                    data.OnRead(entry.item);
                    result[k] = data as T;
                }
            }
            return result;
        }

        private SavableEntry CreateEntry(int typeId, ISavableData data)
        {
            GameSaveItem item = new GameSaveItem();
            data.OnWrite(item);
            SavableEntry entry = new SavableEntry()
            {
                typeId = typeId,
                item = item,
            };

            return entry;
        }
    }
}


