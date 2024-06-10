using Game.UI;
using System.Collections;
using System.Collections.Generic;
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
            if (box == null) return;
            if (ListDB == null)
            {
                LoopBox.UpdateTotalCount(0);
            }
            else
            {
                LoopBox.UpdateTotalCount(ListDB.Count);
            }
        }

        private void InitBox()
        {
            if (LoopBox == null) return;
            LoopBox.Init(OnBoxChange);
        }

        private void OnBoxChange(int min, int max, ListItem[] items)
        {
            if (items == null || items.Length <= 0) return;
            if (ListDB == null || ListDB.Count <= 0) return;
            for (int i = 0; i < items.Length; i++)
            {
                items[i].SetListItem(ListDB[min + i], min + i);
            }
        }
    }
}

