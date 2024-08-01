using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class PackageListItem : ListItem
	{
        public TextView textView;

        protected override void OnDatumChange()
        {
            base.OnDatumChange();
            PackageItemDB db = GetListItem<PackageItemDB>();        
            if (db != null)
            {
                if (textView != null)
                {
                    textView.SetTextByStr(db.name.Value);
                }
            }
        }
    }
}

