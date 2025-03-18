using Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    [ListProxy(NavigationListDefine.Test_List_1)]
    public class Test_List_1_Proxy : NavigationListProxy
    {
        public override bool InFocus(NavigationList list, bool isRefocus, int[] indexs = null)
        {
            Debug.Log("Test_List_1_Proxy");
            return base.InFocus(list, isRefocus, indexs);
        }
    }
}