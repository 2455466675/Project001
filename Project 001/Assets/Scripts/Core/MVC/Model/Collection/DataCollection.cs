

using System;
using System.Collections;
using System.Collections.Generic;

namespace MVC
{
    public sealed class DataCollection : DataModel<CollectionValue>, IEnumerable<DataContainer>
    {
        public override ValueType ValueType => ValueType.Collection;

        public int Count => Value.Count;

        public CollectionValue Value
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
                if (EqualityComparer<CollectionValue>.Default.Equals(this.value, value))
                {
                    return;
                }
                this.value = value;
                NotifyChanged();
            }
        }

        public DataContainer this[int index] => Get(index);
        
        public DataCollection(string key)
        {
            Key = key;
            value = new CollectionValue(NotifyChanged);
        }

        public DataContainer Append(bool isSilent = false)
        {          
            return value.Append(isSilent);
        }

        public DataContainer Append(DataContainer item, bool isSilent = false)
        {
            return value.Append(item, isSilent);
        }

        public DataContainer Get(int index)
        {
            return value.Get(index);
        }

        public DataContainer Find(string key, int value)
        {
            return this.value.Find(key, value);
        }

        public DataContainer Find(string key, bool value)
        {
            return this.value.Find(key, value);
        }

        public DataContainer Find(string key, float value)
        {
            return this.value.Find(key, value);
        }

        public DataContainer Find(string key, string value)
        {
            return this.value.Find(key, value);
        }

        public DataContainer Find(Predicate<DataContainer> match)
        {
            return value.Find(match);
        }

        public List<DataContainer> FindAll(Predicate<DataContainer> match)
        {
            return value.FindAll(match);
        }

        public List<DataContainer> ToList()
        {
            return value.ToList();
        }

        public void Remove(DataContainer item, bool isSilent = false)
        {
            value.Remove(item, isSilent);
        }

        public void RemoveAt(int index, bool isSilent = false)
        {
            value.RemoveAt(index, isSilent);
        }

        public void Clear()
        {
            value.Clear();
        }

        public IEnumerator<DataContainer> GetEnumerator()
        {
            return value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return value.GetEnumerator();
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}

