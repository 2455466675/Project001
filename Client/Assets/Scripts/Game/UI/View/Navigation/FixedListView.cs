using Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class FixedListView : NavigationListView
    {
        public override NavigationList List => list;

        [SerializeField]
        private FixedNavigationList list;

        private void Awake()
        {
            if (list != null) 
            {
                list.Init();                
            }
        }

        public override void UpdateData(List<object> data)
        {            
            if (list == null) 
            {
                return;
            }

            if (data == null) 
            {
                return;
            }

            for (int i = 0; i < data.Count; i++)
            {
                var item = list.GetItem(i);
                if (item != null)
                {
                    item.SetData(data[i]);
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