using System.Collections;
using System.Collections.Generic;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
	public class DataProxy
	{
        private DataContainer container;
        public DataProxy(DataContainer container)
        {
            if (container == null)
            {
                return;
            }
            this.container = container;
            container.SetProxy(this);
        }

        public void SetBaseValue(string key, int value)
        {
            container?.SetBaseValue(key, value);
        }
        public void SetBaseValue(string key, float value)
        {
            container?.SetBaseValue(key, value);
        }
        public void SetBaseValue(string key, bool value)
        {
            container?.SetBaseValue(key, value);
        }
        public void SetBaseValue(string key, string value)
        {
            container?.SetBaseValue(key, value);
        }

        public DataContainer CreateContainer(string key)
        {
            return container?.CreateContainer(key);
        }

        public DataCollection CreateCollection(string key)
        {
            return container?.CreateCollection(key);
        }

        public DataBase GetDataBase(string key)
        {
            return container?.GetDataBase(key);
        }

        public DataContainer GetDataContainer(string key)
        {
            return container?.GetDataContainer(key);
        }

        public DataCollection GetDataCollection(string key)
        {
            return container?.GetDataCollection(key);
        }

        public DataBase FindDataBase(string path)
        {
            return container?.FindDataBase(path);
        }

        public DataContainer FindDataContainer(string path)
        {
            return container?.FindDataContainer(path);
        }

        public DataCollection FindDataCollection(string path)
        {
            return container?.FindDataCollection(path);
        }

        public DataContainer SetContainerLinker(string key, DataContainer container)
        {
            return container.SetContainerLinker(key, container);
        }
    }
}

