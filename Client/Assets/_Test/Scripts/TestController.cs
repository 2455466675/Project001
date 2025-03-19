using Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TestController : Controller
    {
        public FluidNavigationList fluidNavigationList;

        private void Awake()
        {
            if (fluidNavigationList != null)
            {
                fluidNavigationList.Init();
                fluidNavigationList.UpdateItemCount(20);
            }
        }

        public void OnClickTestGroup1(NavigationItem item) 
        {
            GameWorld.Root.GetComponent<UIComponent>().Navigate(NavigationListDefine.Test_List_2);
        }
    }
}
