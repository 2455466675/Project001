using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
	public class DataSet : MonoBehaviour
	{
        private DataContainer datum;

        private List<View> views;

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

        public void SetDatum(DataContainer datum)
        {
            this.datum?.Unbind(OnDatumChanged);
            this.datum = datum;
            this.datum?.Bind(OnDatumChanged);
        }

        public DataBase FindDataBase(string path)
        {
            if (datum == null)
            {
                return null;
            }
            return datum.FindDataBase(path);
        }

        public DataCollection FindDataCollection(string path)
        {
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

            if (datum == null)
            {
                return null;
            }

            return datum.FindDataContainer(path);
        }

        private void OnDatumChanged()
        {
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
    }
}

