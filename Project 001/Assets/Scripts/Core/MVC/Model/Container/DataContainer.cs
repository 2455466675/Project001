using System;
using System.Collections;
using System.Collections.Generic;

namespace MVC
{
    public sealed class DataContainer : DataModel<ContainerValue>, IEnumerable<IDataModel>
    {
        private static DataContainer root;
        public static DataContainer Root
        {
            get 
            {
                return root;
            }
        }

        public static void CreateRoot()
        {
            root ??= new DataContainer("Root");
        }

        public override ValueType ValueType => ValueType.Container;

        public int Count => value.Count;

        public ContainerValue Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                if (EqualityComparer<ContainerValue>.Default.Equals(this.value, value))
                {
                    return;
                }
                this.value = value;
                NotifyChanged();
            }
        }

        private DataProxy proxy;

        public DataContainer(string key)
        {
            Key = key;
            value = new ContainerValue();
        }

        public void SetProxy(DataProxy proxy)
        {            
            this.proxy = proxy;
        }

        public void SetBaseValue(string key, int value)
        {
            this.value.SetBaseValue(key, value);
        }
        public void SetBaseValue(string key, float value)
        {
            this.value.SetBaseValue(key, value);
        }
        public void SetBaseValue(string key, bool value)
        {
            this.value.SetBaseValue(key, value);
        }
        public void SetBaseValue(string key, string value)
        {
            this.value.SetBaseValue(key, value);
        }

        public DataContainer CreateContainer(string key)
        {
            return value.CreateContainer(key);
        }

        public DataCollection CreateCollection(string key)
        {
            return value.CreateCollection(key);
        }

        public DataBase GetDataBase(string key)
        {
            return value.GetDataBase(key);
        }

        public DataContainer GetDataContainer(string key)
        {
            return value.GetDataContainer(key);
        }

        public DataCollection GetDataCollection(string key)
        {
            return value.GetDataCollection(key);
        }

        public DataBase FindDataBase(string path)
        {
            return value.FindDataBase(path);
        }

        public DataContainer FindDataContainer(string path)
        {
            return value.FindDataContainer(path);
        }

        public DataCollection FindDataCollection(string path)
        {
            return value.FindDataCollection(path);
        }

        public DataContainer SetContainerLinker(string key, DataContainer container)
        {
            DataContainer origin = CreateContainer(key);
            if (container != null)
            {
                origin.Value = container.Value;                
            }
            return origin;
        }

        public DataCollection SetCollectionLinker(string key, DataCollection collection)
        {
            DataCollection origin = CreateCollection(key);
            if (collection != null)
            {
                origin.Value = collection.Value;
            }
            return origin;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public IEnumerator<IDataModel> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Value.GetEnumerator();
        }
    }
}

