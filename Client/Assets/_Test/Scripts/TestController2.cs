using Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TestController2 : Controller
    {
        public FluidNavigationList fluidNavigationList;
        public FixedNavigationList fixedNavigationList;

        private void Awake()
        {
            if (fluidNavigationList != null) 
            {
                fluidNavigationList.Init();
                fluidNavigationList.UpdateItemCount(20);
            }

            if (fixedNavigationList != null) 
            {
                fixedNavigationList.Init();
            }
        }

        public void OnClickTestGroup2(NavigationItem item)
        {
            GameWorld.Root.GetComponent<UIComponent>().Navigate(NavigationListDefine.Test_List_3);
        }

        public void OnClickTestGroup3(NavigationItem item)
        {
            Debug.Log("OnClickTestGroup3");
        }
    }
}
