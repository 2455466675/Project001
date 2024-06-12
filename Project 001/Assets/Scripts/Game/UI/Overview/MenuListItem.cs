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
        public TextView textView;

        protected override void OnDatumChange()
        {
            base.OnDatumChange();
            MenuItem menu = GetListItem<MenuItem>();
            if (menu != null) 
            { 
                if (textView != null)
                {
                    textView.SetTextByStr(menu.name);
                }
            }
        }
    }
}

