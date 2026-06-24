using System;
using System.Threading;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace MVC
{
    public class ObservableList<T> : ObservableModel, IList<T>, IList where T : ObservableModel
    {
        private static readonly PropertyChangedEventArgs CountEventArgs = new PropertyChangedEventArgs(nameof(ObservableList<T>.Count));

        private static bool IsCompatibleObject(object value)
        {
            return ((value is T) || (value == null && default(T) == null));
        }

        public int Count => items.Count;

        public bool IsReadOnly => Items.IsReadOnly;

        bool IList.IsFixedSize
        {
            get
            {
                if (items is IList list)
                {
                    return list.IsFixedSize;
                }
                else
                {
                    return IsReadOnly;
                }
            }
        }

        bool IList.IsReadOnly => IsReadOnly;

        int ICollection.Count => Count;

        bool ICollection.IsSynchronized => false;

        object syncRoot;
        object ICollection.SyncRoot
        {
            get
            {
                if (syncRoot == null)
                {
                    if (items is ICollection c)
                    {
                        this.syncRoot = c;
                    }
                    else
                    {
                        Interlocked.CompareExchange(ref this.syncRoot, new object(), null);
                    }
                }

                return this.syncRoot;
            }
        }

        public T this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                if (IsReadOnly)
                    throw new NotSupportedException("ReadOnlyCollection");

                if (index < 0 || index >= items.Count)
                    throw new ArgumentOutOfRangeException(string.Format("ArgumentOutOfRangeException:{0}", index));

                SetItem(index, value);
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                if (value == null && !(default(T) == null))
                    throw new ArgumentNullException("value");

                try
                {
                    this[index] = (T)value;
                }
                catch (InvalidCastException e)
                {
                    throw new ArgumentException("", e);
                }
            }
        }

        private IList<T> Items { get { return items; } }

        private readonly List<T> items;
        public ObservableList()
        {
            items = new List<T>();
        }

        public ObservableList(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("list");

            items = new List<T>(collection);
        }

        public ObservableList(int capacity)
        {
            items = new List<T>(capacity);
        }

        public void Add(T item)
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            InsertItem(items.Count, item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            InsertItem(items.Count, collection);
        }

        public void Clear()
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            ClearItems();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            if (index < 0 || index > items.Count)
                throw new ArgumentOutOfRangeException(string.Format("ArgumentOutOfRangeException:{0}", index));

            InsertItem(index, item);
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            if (index < 0 || index > items.Count)
                throw new ArgumentOutOfRangeException(string.Format("ArgumentOutOfRangeException:{0}", index));

            InsertItem(index, collection);
        }

        public bool Remove(T item)
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            int index = items.IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            else
            {
                RemoveItem(index);
                return true;
            }
        }

        public void RemoveAt(int index)
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            if (index < 0 || index >= items.Count)
                throw new ArgumentOutOfRangeException(string.Format("ArgumentOutOfRangeException:{0}", index));

            RemoveItem(index);
        }

        int IList.Add(object value)
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            if (value == null && !(default(T) == null))
                throw new ArgumentNullException("value");

            try
            {
                Add((T)value);
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException("", e);
            }

            return this.Count - 1;
        }

        void IList.Clear()
        {
            Clear();
        }

        bool IList.Contains(object value)
        {
            if (IsCompatibleObject(value))
            {
                return Contains((T)value);
            }
            else
            {
                return false;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Rank != 1)
                throw new ArgumentException("RankMultiDimNotSupported");

            if (array.GetLowerBound(0) != 0)
                throw new ArgumentException("NonZeroLowerBound");

            if (index < 0)
                throw new ArgumentOutOfRangeException(string.Format("ArgumentOutOfRangeException:{0}", index));

            if (array.Length - index < Count)
                throw new ArgumentException("ArrayPlusOffTooSmall");

            T[] tArray = array as T[];
            if (tArray != null)
            {
                items.CopyTo(tArray, index);
            }
            else
            {
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                    throw new ArgumentException("InvalidArrayType");

                object[] objects = array as object[];
                if (objects == null)
                    throw new ArgumentException("InvalidArrayType");

                int count = items.Count;
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        objects[index++] = items[i];
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException("InvalidArrayType");
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        int IList.IndexOf(object value)
        {
            if (IsCompatibleObject(value))
            {
                return IndexOf((T)value);
            }
            else
            {
                return -1;
            }
        }

        void IList.Insert(int index, object value)
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            if (value == null && !(default(T) == null))
                throw new ArgumentNullException("value");

            try
            {
                Insert(index, (T)value);
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException("", e);
            }
        }

        void IList.Remove(object value)
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            if (IsCompatibleObject(value))
            {
                Remove((T)value);
            }
        }

        void IList.RemoveAt(int index)
        {
            if (IsReadOnly)
                throw new NotSupportedException("ReadOnlyCollection");

            RemoveAt(index);
        }

        private void SetItem(int index, T item)
        {
            items[index] = item;
        }

        private void InsertItem(int index, T item)
        {
            items.Insert(index, item);

            OnPropertyChanged(CountEventArgs);
        }

        private void InsertItem(int index, IEnumerable<T> collection)
        {
            items.InsertRange(index, collection);

            OnPropertyChanged(CountEventArgs);
        }

        private void ClearItems()
        {
            items.Clear();

            OnPropertyChanged(CountEventArgs);
        }

        private void RemoveItem(int index)
        {
            items.RemoveAt(index);

            OnPropertyChanged(CountEventArgs);
        }

        protected virtual void RemoveItem(int index, int count)
        {
            items.RemoveRange(index, count);

            OnPropertyChanged(CountEventArgs);
        }
    }
}


