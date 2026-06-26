using System.Collections.Generic;

namespace GameFramework.Core
{
    internal class GameSaveReader : IGameSaveReader
    {
        private Dictionary<string, IGameSaveItem> items;
        private IGameSaveItem current;

        public void SetData(IGameSaveData data) 
        {
            items = data.GetData();
        }

        public void MoveNext(string groupName)
        {
            items ??= new Dictionary<string, IGameSaveItem>();
            if (items.ContainsKey(groupName))
            {                
                current = items[groupName];
            }
            else
            {
                current = null;
            }
        }

        public int ReadInt(string key)
        {
            if (current == null)
            {
                return 0;
            }
            else
            {
                return current.ReadInt(key);
            }
        }

        public long ReadLong(string key)
        {
            if (current == null)
            {
                return 0;
            }
            else
            {
                return current.ReadLong(key);
            }
        }

        public bool ReadBool(string key)
        {
            if (current == null)
            {
                return false;
            }
            else
            {
                return current.ReadBool(key);
            }
        }

        public float ReadFloat(string key)
        {
            if (current == null)
            {
                return 0f;
            }
            else
            {
                return current.ReadFloat(key);
            }
        }

        public double ReadDouble(string key)
        {
            if (current == null)
            {
                return 0d;
            }
            else
            {
                return current.ReadDouble(key);
            }
        }

        public string ReadString(string key)
        {
            if (current == null)
            {
                return string.Empty;
            }
            else
            {
                return current.ReadString(key);
            }
        }

        public T ReadData<T>(string key) where T : class, ISavableData
        {
            if (current == null)
            {
                return default;
            }
            else
            {
                return current.ReadData<T>(key);
            }
        }

        public List<T> ReadList<T>(string key) where T : class, ISavableData
        {
            if (current == null)
            {
                return new List<T>();
            }
            else
            {
                return current.ReadList<T>(key);
            }
        }

        public Dictionary<int, T> ReadDicWithIntKey<T>(string key) where T : class, ISavableData
        {
            if (current == null)
            {
                return new Dictionary<int, T>();
            }
            else
            {
                return current.ReadDicWithIntKey<T>(key);
            }
        }

        public Dictionary<string, T> ReadDicWithStringKey<T>(string key) where T : class, ISavableData
        {
            if (current == null)
            {
                return new Dictionary<string, T>();
            }
            else
            {
                return current.ReadDicWithStringKey<T>(key);
            }
        }
    }
}

