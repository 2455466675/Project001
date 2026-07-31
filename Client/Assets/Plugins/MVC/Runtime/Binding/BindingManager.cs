using System;
using System.Collections.Generic;

namespace MVC
{
    internal class BindingManager : IBindingManager
    {
        private Dictionary<string, Binder> binders;

        public BindingManager()
        {
            binders = new Dictionary<string, Binder>();
        }

        public Binder CreateBinder(int index)
        {
            return CreateBinder(index.ToString());
        }

        public Binder CreateBinder(string key)
        {
            if (string.Equals(key, Controller.DefaultBinderKey))
            {
                throw new ArgumentException(
                    string.Format("'{0}' 是框架保留的默认 Binder key，禁止业务使用。", key), nameof(key));
            }
            return CreateBinderInternal(key);
        }

        internal Binder CreateBinderInternal(string key)
        {
            Binder binder;
            if (binders.ContainsKey(key))
            {
                binder = binders[key];
                binder.Unbinding();
            }
            else
            {
                binder = new Binder();
                binders.Add(key, binder);
            }
            return binder;
        }

        public void RemoveBinder(int index)
        {
            RemoveBinder(index.ToString());
        }

        public void RemoveBinder(string key)
        {
            if (!binders.ContainsKey(key))
            {
                return;
            }
            Binder binder = binders[key];
            binder.Unbinding();
            binders.Remove(key);
        }

        public void ClearBinding()
        {
            foreach (var item in binders)
            {
                item.Value.Unbinding();
            }
            binders.Clear();
        }
    }
}
