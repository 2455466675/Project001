using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    [Serializable]
    public class GameSaveItem : IGameSaveItem
    {
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
        private Dictionary<string, bool> boolValues;
        [SerializeField]
        private Dictionary<string, double> doubleValues;
        [SerializeField]
        private Dictionary<string, string> stringValues;
        [SerializeField]
        private Dictionary<string, object> datas;
        [SerializeField]
        private Dictionary<string, List<object>> dataLists;
        [SerializeField]
        private Dictionary<string, SerializableItem<int>> serializablesIntKey;
        [SerializeField]
        private Dictionary<string, SerializableItem<string>> serializablesStrKey;

        public GameSaveItem()
        {
            intValues = new Dictionary<string, int>();
            boolValues = new Dictionary<string, bool>();
            doubleValues = new Dictionary<string, double>();
            stringValues = new Dictionary<string, string>();
            datas = new Dictionary<string, object>();
            dataLists = new Dictionary<string, List<object>>();
            serializablesIntKey = new Dictionary<string, SerializableItem<int>>();
            serializablesStrKey = new Dictionary<string, SerializableItem<string>>();
        }

        public void Write(string key, int value)
        {
            intValues[key] = value;
        }

        public void Write(string key, bool value)
        {
            boolValues[key] = value;
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

        public void Write<T>(string key, T value) where T : class
        {
            datas[key] = value;
        }

        public void Write<T>(string key, List<T> value) where T : class
        {
            List<object> datas = new List<object>();
            foreach (var item in value)
            {
                datas.Add(item);
            }
            dataLists[key] = datas;
        }

        public void Write<T>(string key, Dictionary<int, T> value) where T : class
        {
            SerializableItem<int> serializable = new SerializableItem<int>();
            foreach (var item in value)
            {
                serializable.keys.Add(item.Key);
                serializable.values.Add(item.Value);
            }
            serializablesIntKey[key] = serializable;
        }

        public void Write<T>(string key, Dictionary<string, T> value) where T : class
        {
            SerializableItem<string> serializable = new SerializableItem<string>();
            foreach (var item in value)
            {
                serializable.keys.Add(item.Key);
                serializable.values.Add(item.Value);
            }
            serializablesStrKey[key] = serializable;
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

        public bool ReadBool(string key)
        {
            if (boolValues.ContainsKey(key))
            {
                return boolValues[key];
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

        public T ReadData<T>(string key) where T : class
        {
            if (datas.ContainsKey(key))
            {
                return datas[key] as T;
            }
            else
            {
                return default;
            }
        }

        public List<T> ReadList<T>(string key) where T : class
        {
            List<T> result = new List <T>();
            if (dataLists.ContainsKey(key))
            {
                foreach (var item in dataLists[key])
                {
                    result.Add(item as T);
                }
            }
            return result;
        }

        public Dictionary<int, T> ReadDicWithIntKey<T>(string key) where T : class
        {
            Dictionary<int, T> result = new Dictionary<int, T>();
            if (serializablesIntKey.TryGetValue(key, out var serializableItem))
            {
                for (int i = 0; i < serializableItem.Count; i++)
                {
                    result[serializableItem.keys[i]] = serializableItem.values[i] as T;
                }
            }
            return result;
        }

        public Dictionary<string, T> ReadDicWithStringKey<T>(string key) where T : class
        {
            Dictionary<string, T> result = new Dictionary<string, T>();
            if (serializablesStrKey.TryGetValue(key, out var serializableItem))
            {
                for (int i = 0; i < serializableItem.Count; i++)
                {
                    result[serializableItem.keys[i]] = serializableItem.values[i] as T;
                }
            }
            return result;
        }
    }
}


