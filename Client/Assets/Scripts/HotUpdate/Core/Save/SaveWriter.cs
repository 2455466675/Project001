using System.Collections.Generic;

namespace GameFramework.Core
{
    internal class SaveWriter : ISaveWriter
    {
        private Dictionary<string, SaveItem> items;
        private SaveItem current;

        internal SaveWriter()
        {
            items = new Dictionary<string, SaveItem>();
        }

        public void Next(string groupName)
        {
            SaveItem group = new SaveItem();
            items[groupName] = group;
            current = group;
        }

        public SaveData GetData()
        {
            SaveData data = new SaveData();
            data.items = items;
            return data;
        }

        public void Write(string key, int value)
        {
            current.Write(key, value);
        }

        public void Write(string key, bool value)
        {
            current.Write(key, value);
        }

        public void Write(string key, string value)
        {
            current.Write(key, value);
        }

        public void Write(string key, DataModel value)
        {
            current.Write(key, value);
        }

        public void Write(string key, IList<DataModel> value)
        {
            current.Write(key, value);
        }

        public void Write(string key, IReadOnlyDictionary<int, DataModel> value)
        {
            current.Write(key, value);
        }

        public void Write(string key, IReadOnlyDictionary<string, DataModel> value)
        {
            current.Write(key, value);
        }
    }
}
