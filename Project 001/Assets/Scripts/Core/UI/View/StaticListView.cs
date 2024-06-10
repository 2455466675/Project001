using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class StaticListView : ListView
	{
        public StaticNavigationBox StaticBox
        {
            get
            {
                if (box == null)
                {
                    box = GetComponent<StaticNavigationBox>();
                }
                if (box == null)
                {
                    return null;
                }
                return box as StaticNavigationBox;
            }
        }

        public override void UpdateView()
        {
            if (StaticBox == null) return;
            if (!IsValid) return;
            if (ListDB == null) return;
            
            for (int i = 0; i < ListDB.Count; i++) 
            {
                ListItem listItem = StaticBox.GetListItem(i);
                if (listItem != null)
                {
                    listItem.SetListItem(ListDB[i], i);
                }
            }
        }
    }
}

