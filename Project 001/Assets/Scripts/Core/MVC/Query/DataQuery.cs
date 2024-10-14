using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent (typeof (DataSet))]
	public class DataQuery : MonoBehaviour
	{
        public DataQuery parent;
        public DataSet dataSet;
        public string dataPath;

        private DataContainer datum;
        private bool isQueried;
        private event Action OnDatumChangedEvent;
     
        private void Start()
        {
            if (parent != null)
            {
                parent.Bind(OnParentDatumChanged);
            }
            Query();
        }

        private void OnDestroy()
        {
            if (parent != null)
            {
                parent.Unbind(OnParentDatumChanged);
                parent = null;
            }
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

        private DataContainer FindDataContainer(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (datum == null)
            {
                if (isQueried)
                {
                    return null;
                }
                else
                {
                    Query();
                    isQueried = true;
                }
            }

            if (datum == null)
            {
                return null;
            }

            return datum.FindDataContainer(path);
        }

        private void Query()
        {
            if (string.IsNullOrEmpty(dataPath))
            {
                return;
            }
            if (isQueried)
            {
                return;
            }
            if (parent == null)
            {
                datum = DataContainer.Root.FindDataContainer(dataPath);
            }
            else
            {
                datum = parent.FindDataContainer(dataPath);
            }
            if (datum == null)
            {
                return;
            }
            datum.Bind(OnDatumChanged, true);
            isQueried = true;
            if (dataSet != null)
            {
                dataSet.SetDatum(datum);
            }
        }

        private void OnDatumChanged()
        {
            OnDatumChangedEvent?.Invoke();
        }

        private void OnParentDatumChanged()
        {
            isQueried = false;
            Query();
        }

        private void OnValidate()
        {
            dataSet = GetComponent<DataSet>();
        }
    }
}

