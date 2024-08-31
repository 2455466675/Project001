using Game.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class MenuListItem : GuidableItem<OverviewMainMenuItemDB>
	{   
        protected override void OnDatumChange()
        {
            TextView textView = GetView<TextView>("buttonName");
            if (textView == null)
            {
                return;
            }
            textView.SetDatum(Dautm.name);
        }
    }
}

