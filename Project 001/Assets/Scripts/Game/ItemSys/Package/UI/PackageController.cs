using Game.Cfg;
using Game.Core;
using Game.System;
using Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class PackageController : MonoBehaviour
	{
        public LoopListView list;

        public void OnClick(GuidableItem item)
        {
            GameCore.UI.Enter(ListName.PackageList);
        }

        public void OnSelected(GuidableItem item)
        {
            string name = item.gameObject.name;
            if (name == "Button (1)")
            {
                list.Query(0);
            }
            else if (name == "Button (2)")
            {
                list.Query(1);
            }
            else if (name == "Button (3)")
            {
                list.Query(2);
            }
            else if (name == "Button (4)")
            {
                list.Query(3);
            }
            else
            {
                list.Query(4);
            }
        }    
    }
}

