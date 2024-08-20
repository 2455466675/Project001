using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class PackageListItem : GuidableItemBase
	{
        public TextView textView;

        protected override void OnDatumChange()
        {
            base.OnDatumChange();
            PackageItemDB db = GetItemDB<PackageItemDB>();        
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

