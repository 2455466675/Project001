using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameFramework.Core
{
    [Serializable]
    public class SaveItem 
    {
        [Serializable]
        private class SerializableItem<T>
        {
            public List<T> keys = new List<T>();
            public List<DataModel> values = new List<DataModel>();
        }

        [SerializeField]
        private Dictionary<string, int> intValues;
        [SerializeField]
        private Dictionary<string, bool> boolValues;
        [SerializeField]
        private Dictionary<string, string> stringValues;
        [SerializeField]
        private Dictionary<string, DataModel> datas;
        [SerializeField]
        private Dictionary<string, List<DataModel>> dataLists;
        [SerializeField]
        private Dictionary<string, SerializableItem<int>> serializables1;
        [SerializeField]
        private Dictionary<string, SerializableItem<string>> serializables2;

        public SaveItem()
        {
            intValues = new Dictionary<string, int>();
            boolValues = new Dictionary<string, bool>();
            stringValues = new Dictionary<string, string>();
            datas = new Dictionary<string, DataModel>();
            dataLists = new Dictionary<string, List<DataModel>>();
            serializables1 = new Dictionary<string, SerializableItem<int>>();
            serializables2 = new Dictionary<string, SerializableItem<string>>();
        }

        #region Write
        public void Write(string key, int value)
        {
            intValues[key] = value;
        }

        public void Write(string key, bool value)
        {
            boolValues[key] = value;
        }

        public void Write(string key, string value)
        {
            stringValues[key] = value;
        }

        public void Write(string key, DataModel value)
        {
            datas[key] = value;
        }

        public void Write(string key, IList<DataModel> value)
        {
            List<DataModel> datas = new List<DataModel>();
            foreach (var item in value)
            {
                datas.Add(item);
            }
            dataLists[key] = datas;
        }

        public void Write(string key, IReadOnlyDictionary<int, DataModel> value) 
        {
            SerializableItem<int> serializable = new SerializableItem<int>();
            foreach (var item in value)
            {
                serializable.keys.Add(item.Key);
                serializable.values.Add(item.Value);
            }
            serializables1[key] = serializable;
        }

        public void Write(string key, IReadOnlyDictionary<string, DataModel> value)
        {
            SerializableItem<string> serializable = new SerializableItem<string>();
            foreach (var item in value)
            {
                serializable.keys.Add(item.Key);
                serializable.values.Add(item.Value);
            }
            serializables2[key] = serializable;
        }
        #endregion

        #region Read
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

        public DataModel ReadData(string key)
        {
            if (datas.ContainsKey(key))
            {
                return datas[key];
            }
            else
            {
                return new DataModel();
            }
        }

        public List<DataModel> ReadList(string key)
        {
            if (dataLists.ContainsKey(key))
            {
                return dataLists[key];
            }
            else
            {
                return new List<DataModel>();
            }
        }

        public Dictionary<int, DataModel> ReadDicWithIntKey(string key) 
        {
            Dictionary<int, DataModel> result = new Dictionary<int, DataModel>();
            if (serializables1.TryGetValue(key, out var serializableItem)) 
            {
                List<int> keys = serializableItem.keys;
                List<DataModel> values = serializableItem.values;

                for (int i = 0; i < keys.Count; i++)
                {
                    result[keys[i]] = values[i];
                }
            }

            return result;
        }

        public Dictionary<string, DataModel> ReadDicWithStringKey(string key)
        {
            Dictionary<string, DataModel> result = new Dictionary<string, DataModel>();
            if (serializables2.TryGetValue(key, out var serializableItem))
            {
                List<string> keys = serializableItem.keys;
                List<DataModel> values = serializableItem.values;

                for (int i = 0; i < keys.Count; i++)
                {
                    result[keys[i]] = values[i];
                }
            }

            return result;
        }
        #endregion

        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    return sb.ToString();
        //}
    }
}