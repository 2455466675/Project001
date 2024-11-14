using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
	public class CollectionValue : DataValue, IBindable, IEnumerable<DataContainer>
    {
        public int Count => collection.Count;

        private List<DataContainer> collection;

        private event Action OnValueChanged;
        public CollectionValue(Action onValueChanged)
        {
            collection = new List<DataContainer>();
            Bind(onValueChanged);
        }

        public DataContainer Append(bool isSilent = false)
        {
            DataContainer item = new DataContainer(string.Empty);            
            return Append(item, isSilent);
        }

        public DataContainer Append(DataContainer item, bool isSilent = false)
        {
            if ( item == null )
            {
                return null;
            }
            collection.Add(item);
            if (!isSilent)
            {
                NotifyChanged();
            }
            return item;
        }

        public DataContainer Get(int index)
        {
            if (index < 0 || index >= collection.Count)
            {
                return null;
            }
            return collection[index];
        }

        public DataContainer Find(string key, int value)
        {
            return collection.Find((d) => d.GetDataBase(key).IntValue == value);
        }

        public DataContainer Find(string key, bool value)
        {
            return collection.Find((d) => d.GetDataBase(key).BoolValue == value);
        }

        public DataContainer Find(string key, float value)
        {
            return collection.Find((d) => d.GetDataBase(key).FloatValue == value);
        }

        public DataContainer Find(string key, string value)
        {
            return collection.Find((d) => d.GetDataBase(key).StringValue == value);
        }

        public DataContainer Find(Predicate<DataContainer> match)
        {
            return collection.Find(match);
        }

        public List<DataContainer> FindAll(Predicate<DataContainer> match)
        {
            return collection.FindAll(match);
        }

        public List<DataContainer> ToList()
        {
            return collection;
        }

        public void Remove(DataContainer item, bool isSilent = false)
        {
            if (collection.Remove(item) && !isSilent)
            {
                NotifyChanged();
            }
        }

        public void RemoveAt(int index, bool isSilent = false)
        {
            if (index < 0 || index >= collection.Count)
            {
                return;
            }
            collection.RemoveAt(index);
            if (!isSilent)
            {
                NotifyChanged();
            }
        }

        public void Clear()
        {
            collection.Clear();
            NotifyChanged();
        }

        public void Bind(Action action, bool isSilent = true)
        {
            if (action == null)
            {
                return;
            }
            OnValueChanged += action;
            if (!isSilent)
            {
                action.Invoke();
            }
        }

        public void Unbind(Action action)
        {
            if (action == null)
            {
                return;
            }
            OnValueChanged -= action;
        }

        private void NotifyChanged()
        {
            OnValueChanged?.Invoke();
        }

        public IEnumerator<DataContainer> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append($"\tcount : {collection.Count}");
            stringBuilder.Append(Environment.NewLine);
            for (int i = 0; i < collection.Count; i++)
            {
                stringBuilder.Append($"\t{collection[i]}");
                stringBuilder.Append(Environment.NewLine);
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}

