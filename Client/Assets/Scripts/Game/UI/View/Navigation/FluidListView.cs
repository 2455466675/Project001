using Navigation;
using UnityEngine;

namespace Game.UI
{
    public class FluidListView : NavigationListView
    {
        protected override NavigationList List => list;

        private object[] data;

        [SerializeField]
        private FluidNavigationList list;

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
            this.data = data;
            if (list != null) 
            {
                list.UpdateItemCount(this.data != null ? this.data.Length : 0);
            }
        }

        protected override void Register()
        {
            base.Register();
            list.OnListChangedEvent += List_OnListChangedEvent;
        }

        private void List_OnListChangedEvent(ListChangedEventArgs obj)
        {
            if (data == null || data.Length == 0) 
            {
                return;
            }

            var items = obj.Items;
            if (items == null || items.Length == 0) 
            {
                return;
            }   

            int min = obj.MinIndex;
            int max = obj.MaxIndex;

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                int index = min + i;
                if (index > max || index < 0 || index >= data.Length) 
                {
                    continue;                
                }

                if (item.IsBinded) 
                {
                    OnItemUnbindData(item);
                    item.UnbindData();
                }

                item.BindData(data[index]); 
                OnItemBindData(item);

                OnItemRefresh(item);
            }
        }

        private void OnValidate()
        {
            if (list == null)
            {
                list = GetComponent<FluidNavigationList>();
            }
        }        
    }
}