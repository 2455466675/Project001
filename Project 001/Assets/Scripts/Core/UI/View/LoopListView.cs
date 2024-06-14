using Game.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
	public class LoopListView : ListView
	{
        public LoopNavigationBox LoopBox 
        {
            get 
            {
                if (box == null)
                {
                    box = GetComponent<LoopNavigationBox>();
                }
                if (box == null)
                {
                    return null;
                }
                return box as LoopNavigationBox;
            }
        }

        public void Awake()
        {
            InitBox();
        }

        public override void UpdateView()
        {
            base.UpdateView();
            if (LoopBox == null) return;
            Debug.Log($"UpdateView:{Count}");
            LoopBox.UpdateTotalCount(Count);
        }

        private void InitBox()
        {
            if (LoopBox == null) return;
            LoopBox.Init(OnBoxChange);
        }

        private void OnBoxChange(int min, int max, ListItem[] listItems)
        {
            Debug.Log($"Count:{Count}");
            if (listItems == null || listItems.Length <= 0) return;
            if (Count <= 0) return;
            
            for (int i = 0; i < listItems.Length; i++)
            {
                listItems[i].SetListItem(items[min + i], min + i);
            }
        }
    }
}

