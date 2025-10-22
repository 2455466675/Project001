using System.Collections.Generic;

namespace GameFramework.Core 
{
    internal class SaveReader : ISaveReader
    {
        private Dictionary<string, SaveItem> items;
        private SaveItem current;
        public void Next(string groupName)
        {
            items ??= new Dictionary<string, SaveItem>();

            if (!items.ContainsKey(groupName))
            {
                current = new SaveItem();
            }
            else 
            {
                current = items[groupName];            
            }
        }

        public void SetData(SaveData data)
        {
            this.items = data.items;
        }

        public int ReadInt(string key)
        {
            return current.ReadInt(key);
        }

        public bool ReadBool(string key)
        {
            return current.ReadBool(key);
        }

        public string ReadString(string key)
        {
            return current.ReadString(key);
        }

        public DataModel ReadData(string key)
        {
            return current.ReadData(key);
        }

        public List<DataModel> ReadList(string key)
        {
            return current.ReadList(key);
        }

        public Dictionary<int, DataModel> ReadDicWithIntKey(string key)
        {
            return current.ReadDicWithIntKey(key);
        }

        public Dictionary<string, DataModel> ReadDicWithStringKey(string key)
        {
            return current.ReadDicWithStringKey(key);
        }
    }
}
