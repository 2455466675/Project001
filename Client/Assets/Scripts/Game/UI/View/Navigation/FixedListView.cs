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

        public override void UpdateData(INavigationItemData[] data)
        {            
            if (list == null) 
            {
                return;
            }

            if (data == null) 
            {
                return;
            }
            
            for (int i = 0; i < list.Count; i++)
            {
                var item = list.GetItem(i);
                if (item == null) 
                {
                    continue;
                }

                if (i < data.Length) 
                {
                    if (item.IsBinded)
                    {
                        OnItemUnbindData(item);
                        item.UnbindData();
                    }

                    item.BindData(data[i]);
                    OnItemBindData(item);

                    OnItemRefresh(item);
                }
                else
                {
                    OnItemUnbindData(item);
                    item.UnbindData();
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