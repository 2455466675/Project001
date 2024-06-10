using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class ListDB : DataBase
    {
        private List<ItemDB> value;

        public ListDB()
        {
            value = new List<ItemDB>();
        }

        public ListDB(IEnumerable<ItemDB> v)
        {
            value = new List<ItemDB>(v);
        }

        public ItemDB this[int index]
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

        public int Count => value.Count;

        public void Add(ItemDB v)
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

        public T Find<T>(Predicate<ItemDB> predicate) where T : ItemDB
        {
            return (T)value.Find(predicate);
        }

        public List<ItemDB> FindAll(Predicate<ItemDB> predicate)
        {
            return value.FindAll(predicate);
        }

        public ItemDB[] ToArray()
        {
            return value.ToArray();
        }

        public List<ItemDB> ToList()
        {
            return new List<ItemDB>(value);
        }

        public override string ToString()
        {
            return Count.ToString();
        }
    }
}

