using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Game.Core
{

    public abstract class ListDataBase : DataBase, IEnumerable<ItemDB>
    {

        public abstract int Count();

        public abstract IEnumerator<ItemDB> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ListDB<T> : ListDataBase where T : ItemDB
    {
        private readonly List<T> value;

        public ListDB()
        {
            value = new List<T>();
        }

        public ListDB(IEnumerable<T> v)
        {
            value = new List<T>(v);
        }

        public T this[int index]
        {
            get
            {
                return value[index];
            }
            set
            {
                if (index < 0 || index >= this.value.Count)
                {
                    return;
                }
                this.value[index] = value;
            }
        }

        public override int Count()
        {
            return value.Count;
        }

        public void Add(T v)
        {
            if (v == null) return;
            value.Add(v);
            NotifyChange();
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= value.Count) return;
            value.RemoveAt(index);            
            NotifyChange();
        }

        public void Clear()
        {
            value.Clear();
            NotifyChange();
        }

        public T Find(Predicate<T> predicate) 
        {
            return (T)value.Find(predicate);
        }

        public List<T> FindAll(Predicate<T> predicate)
        {
            return value.FindAll(predicate);
        }

        public T[] ToArray()
        {
            return value.ToArray();
        }

        public List<T> ToList()
        {
            return new List<T>(value);
        }

        public override string ToString()
        {
            return Count().ToString();
        }

        public override IEnumerator<ItemDB> GetEnumerator()
        {
            return value.GetEnumerator();
        }
    }
}

