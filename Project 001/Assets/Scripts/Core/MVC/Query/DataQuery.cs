using UnityEngine;

namespace MVC
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent (typeof (DataSet))]
	public class DataQuery : MonoBehaviour
	{
        [SerializeField]
        private DataSet parent;
        [SerializeField]
        private DataSet dataSet;
        [SerializeField]
        private string dataPath;
        public bool IsQueried { get; private set; }

        private void Awake()
        {
            if (dataSet != null)
            {
                dataSet.SetQuery(this);
            }
        }

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

        public void Query()
        {
            if (string.IsNullOrEmpty(dataPath))
            {
                return;
            }
            if (IsQueried)
            {
                return;
            }

            DataContainer datum;

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

            IsQueried = true;
            if (dataSet != null)
            {
                dataSet.SetDatum(datum);
            }
        }

        private void OnParentDatumChanged()
        {
            IsQueried = false;
            Query();
        }

        private void OnValidate()
        {
            dataSet = GetComponent<DataSet>();
        }
    }
}

