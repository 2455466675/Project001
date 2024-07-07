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
            MenuItem menu = GetListItem<MenuItem>();
            if (menu == null) 
            { 
                return;
            }
            TextView textView = GetView<TextView>("buttonName");
            if (textView == null)
            {
                return;
            }
            textView.SetTextByStr(menu.name);
        }
    }
}

