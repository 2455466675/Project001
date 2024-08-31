using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class PackageListItem : GuidableItem<PackageItemDB>
	{
        public TextView textView;

        protected override void OnDatumChange()
        {         
            if (textView == null)
            {
                return;
            }
            textView.SetTextByStr(Dautm.name.Value);
        }
    }
}

