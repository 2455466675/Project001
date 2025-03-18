using Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [GroupProxy(NavigationGroupDefine.Test_Group_1)]
    public class Test_Group_1_Proxy : NavigationGroupProxy
    {
        public override void InFocus(NavigationGroup group, bool isRefocus)
        {
            base.InFocus(group, isRefocus);
            Debug.Log("Test_Group_1_Proxy");
        }
    }
}