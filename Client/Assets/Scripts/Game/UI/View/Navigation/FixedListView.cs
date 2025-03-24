using Navigation;
using UnityEngine;

namespace Game.UI
{
    public class FixedListView : NavigationListView
    {
        protected override NavigationList List => list;

        [SerializeField]
        private FixedNavigationList list;

        private void Awake()
        {
            if (list != null) 
            {
                list.Init();
                Register();
            }
        }

        public override void UpdateData(object[] data)
        {            
            if (list == null) 
            {
                return;
            }

            if (data == null) 
            {
                return;
            }

            for (int i = 0; i < data.Length; i++)
            {
                var item = list.GetItem(i);
                if (item != null)
                {
                    item.SetData(data[i]);
                    OnItemRefresh(item);
                }
            }       
        }

        private void OnValidate()
        {
            if (list == null)
            {
                list = GetComponent<FixedNavigationList>();
            }
        }
    }
}