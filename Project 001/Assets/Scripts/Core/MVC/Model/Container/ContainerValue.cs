using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
	public class ContainerValue : DataValue
	{
        private Dictionary<string, IDataModel> container;

        public ContainerValue()
        {
            container = new Dictionary<string, IDataModel>();
        }

        public void SetBaseValue(string key, int value)
        {
            DataBase db = GetDataBase(key);
            if (db != null)
            {
                db.IntValue = value;
            }
            else
            {
                container[key] = new DataBase(value);
            }
        }

        public void SetBaseValue(string key, float value)
        {
            DataBase db = GetDataBase(key);
            if (db != null)
            {
                db.FloatValue = value;
            }
            else
            {
                container[key] = new DataBase(value);
            }        
        }
 
        public void SetBaseValue(string key, bool value)
        {
            DataBase db = GetDataBase(key);
            if (db != null)
            {
                db.BoolValue = value;
            }
            else
            {
                container[key] = new DataBase(value);
            }
        }

        public void SetBaseValue(string key, string value)
        {
            DataBase db = GetDataBase(key);
            if (db != null)
            {
                db.StringValue = value;
            }
            else
            {
                container[key] = new DataBase(value);
            }
        }

        public DataContainer CreateContainer(string key)
        {
            DataContainer container = GetDataContainer(key);
            if (container == null)
            {
                container = new DataContainer();
                this.container[key] = container;
            }
            return container;
        }

        public DataCollection CreateCollection(string key)
        {
            DataCollection collection = GetDataCollection(key);
            if (collection == null)
            {
                collection = new DataCollection();
                container[key] = collection;
            }
            return collection;          
        }

        public DataBase GetDataBase(string key)
        {
            if (container.TryGetValue(key, out IDataModel model))
            {
                return model as DataBase;            
            }
            return null;
        }

        public DataContainer GetDataContainer(string key)
        {
            if (container.TryGetValue(key, out IDataModel model))
            {
                return model as DataContainer;
            }
            return null;
        }

        public DataCollection GetDataCollection(string key)
        {
            if (container.TryGetValue(key, out IDataModel model))
            {
                return model as DataCollection;
            }
            return null;
        }

        public DataBase FindDataBase(string path)
        {
            string[] paths = path.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (paths.Length <= 0)
            {
                return null;
            }
            if (paths.Length == 1)
            {
                return GetDataBase(paths[0]);
            }

            DataContainer container = GetDataContainer(paths[0]);

            for (int i = 1; i < paths.Length; i++)
            {
                if (container == null)
                {
                    return null;
                }

                if (i < paths.Length - 1)
                {
                    container = container.GetDataContainer(paths[i]);                    
                }
                else
                {
                    return container.GetDataBase(paths[i]);
                }
            }
            return null;
        }

        public DataContainer FindDataContainer(string path)
        {
            string[] paths = path.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (paths.Length <= 0)
            {
                return null;
            }
            if (paths.Length == 1)
            {
                return GetDataContainer(paths[0]);
            }

            DataContainer container = GetDataContainer(paths[0]);

            for (int i = 1; i < paths.Length; i++)
            {
                if (container == null)
                {
                    return null;
                }
                container = container.GetDataContainer(paths[i]);
            }
            return container;
        }

        public DataCollection FindDataCollection(string path)
        {
            string[] paths = path.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (paths.Length <= 0)
            {
                return null;
            }
            if (paths.Length == 1)
            {
                return GetDataCollection(paths[0]);
            }

            DataContainer container = GetDataContainer(paths[0]);

            for (int i = 1; i < paths.Length; i++)
            {
                if (container == null)
                {
                    return null;
                }

                if (i < paths.Length - 1)
                {
                    container = container.GetDataContainer(paths[i]);
                }
                else
                {
                    return container.GetDataCollection(paths[i]);
                }
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"count : {container.Count}");
            stringBuilder.Append(Environment.NewLine);

            foreach (var item in container)
            {
                stringBuilder.Append($"key : {item.Key}, value : {item.Value}");
                stringBuilder.Append(Environment.NewLine);
            }
       
            return stringBuilder.ToString();
        }
    }
}

