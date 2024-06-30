using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class ItemTypeListItem : ListItem
	{
        public TextView textView;

        protected override void OnDatumChange()
        {
            base.OnDatumChange();
            ItemTypeItemDB db = GetListItem<ItemTypeItemDB>();
            if (db != null)
            {
                if (textView != null)
                {
                    textView.SetTextByStr(db.name);
                }
            }
        }
    }
}

