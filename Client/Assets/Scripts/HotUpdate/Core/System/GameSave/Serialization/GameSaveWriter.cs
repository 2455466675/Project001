using System.Collections.Generic;

namespace GameFramework.Core
{
    internal class GameSaveWriter : IGameSaveWriter
    {
        private Dictionary<string, IGameSaveItem> items;
        private IGameSaveItem current;

        internal GameSaveWriter()
        {
            items = new Dictionary<string, IGameSaveItem>();
            current = null;
        }

        public IGameSaveData GetData() 
        {
            IGameSaveData data = new GameSaveData();
            data.SetData(items);
            return data;
        }

        public void MoveNext(string groupName)
        {
            IGameSaveItem item = new GameSaveItem();
            items[groupName] = item;
            current = item;
        }

        public void Write(string key, int value)
        {
            if (current == null)
            {
                return;
            }
            current.Write(key, value);
        }

        public void Write(string key, long value)
        {
            if (current == null)
            {
                return;
            }
            current.Write(key, value);
        }

        public void Write(string key, bool value)
        {
            if (current == null)
            {
                return;
            }
            current.Write(key, value);
        }

        public void Write(string key, float value)
        {
            if (current == null)
            {
                return;
            }
            current.Write(key, value);
        }

        public void Write(string key, double value)
        {
            if (current == null)
            {
                return;
            }
            current.Write(key, value);
        }

        public void Write(string key, string value)
        {
            if (current == null)
            {
                return;
            }
            current.Write(key, value);
        }

        public void Write(string key, ISavableData value)
        {
            if (current == null)
            {
                return;
            }
            current.Write(key, value);
        }

        public void Write<T>(string key, List<T> value) where T : class, ISavableData
        {
            if (current == null)
            {
                return;
            }
            current.Write(key, value);
        }

        public void Write<T>(string key, Dictionary<int, T> value) where T : class, ISavableData
        {
            if (current == null)
            {
                return;
            }
            current.Write(key, value);
        }

        public void Write<T>(string key, Dictionary<string, T> value) where T : class, ISavableData
        {
            if (current == null)
            {
                return;
            }
            current.Write(key, value);
        }
    }
}