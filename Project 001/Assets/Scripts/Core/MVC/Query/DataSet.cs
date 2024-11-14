using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
	public class DataSet : MonoBehaviour
	{
        public DataContainer Datum => datum;
        private DataContainer datum;

        private DataQuery query;
        private List<View> views;

        private event Action OnDatumChangedEvent;

        private void OnDestroy()
        {
            if (datum == null)
            {
                return;
            }
            datum.Unbind(OnDatumChanged);
            datum = null;
            views?.Clear();
            views = null;
        }

        public void Bind(Action action)
        {
            if (action == null)
            {
                return;
            }
            OnDatumChangedEvent += action;
        }

        public void Unbind(Action action)
        {
            if (action == null)
            {
                return;
            }
            OnDatumChangedEvent -= action;
        }

        public void Register(View view)
        {
            views ??= new List<View>();
            if (view == null)
            {
                return;
            }
            if (views.Contains(view))
            {
                return;
            }
            views.Add(view);
            view.UpdateViewField();
        }

        public void Unregister(View view)
        {
            if (views == null)
            {
                return;
            }
            if (view == null)
            {
                return;
            }
            views.Remove(view);
        }

        public void SetQuery(DataQuery query)
        {
            this.query = query;
        }

        public void SetDatum(DataContainer datum)
        {
            this.datum?.Unbind(OnDatumChanged);
            this.datum = datum;
            this.datum?.Bind(OnDatumChanged);
        }

        public DataBase FindDataBase(string path)
        {
            CheckQuery();
            if (datum == null)
            {
                return null;
            }
            return datum.FindDataBase(path);
        }

        public DataCollection FindDataCollection(string path)
        {
            CheckQuery();
            if (datum == null)
            {
                return null;
            }
            return datum.FindDataCollection(path);
        }

        public DataContainer FindDataContainer(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            CheckQuery();
            if (datum == null)
            {
                return null;
            }
            return datum.FindDataContainer(path);
        }

        private void OnDatumChanged()
        {
            OnDatumChangedEvent?.Invoke();

            if (views == null)
            {
                return;
            }
            foreach (var view in views)
            {
                if (view != null)
                {
                    view.UpdateViewField();
                }
            }
        }

        private void CheckQuery()
        {
            if (query == null || query.IsQueried)
            {
                return;
            }
            query.Query();
        }

        [Button("Print")]
        private void Print()
        {
            if (datum == null) 
            {
                return;
            }

            Debug.Log(datum.ToString());
        }
    }
}

