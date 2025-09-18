using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UI 
{
    [NavigationController(NavigationDefine.TestList1)]
    public class TestNavigationController_1 : NavigationController
    {
        protected override void OnShow()
        {
            MDebug.Log("NavigationDefine.TestList1 OnShow");
        }
    }

    [NavigationController(NavigationDefine.TestList2)]
    public class TestNavigationController_2 : NavigationController
    {
        protected override void OnShow()
        {
            MDebug.Log("NavigationDefine.TestList2 OnShow");
        }
    }
}