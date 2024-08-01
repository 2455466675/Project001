using Game.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class MenuListItem : ListItem
	{   
        protected override void OnDatumChange()
        {
            base.OnDatumChange();
            OverviewMainMenuItem menu = GetListItem<OverviewMainMenuItem>();
            if (menu == null) 
            { 
                return;
            }
            TextView textView = GetView<TextView>("buttonName");
            if (textView == null)
            {
                return;
            }
            textView.SetDatum(menu.name);
        }
    }
}

