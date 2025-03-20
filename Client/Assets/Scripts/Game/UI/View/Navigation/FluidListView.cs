using Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class FluidListView : NavigationListView
    {
        public override NavigationList List => list;

        private List<object> data;

        [SerializeField]
        private FluidNavigationList list;

        private void Awake()
        {
            if (list != null) 
            {
                list.Init();
                list.OnListChangedEvent += List_OnListChangedEvent;
            }
        }

        public override void UpdateData(List<object> data) 
        {
            this.data = data;
            if (list != null) 
            {
                list.UpdateItemCount(this.data != null ? this.data.Count : 0);
            }
        }

        private void List_OnListChangedEvent(ListChangedEventArgs obj)
        {
            if (data == null || data.Count == 0) 
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
                if (index > max || index < 0 || index >= data.Count) 
                {
                    continue;                
                }
                
                item.SetData(data[index]); 
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