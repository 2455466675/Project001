using System;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    public sealed class DataModelList<T> : MVC.ObservableList<T> where T : DataModel
    {
        public DataModelList() : base()
        {
        }

        public DataModelList(IEnumerable<T> collection) : base(collection)
        {
        }

        public DataModelList(int capacity) : base(capacity)
        {
        }
    }
}