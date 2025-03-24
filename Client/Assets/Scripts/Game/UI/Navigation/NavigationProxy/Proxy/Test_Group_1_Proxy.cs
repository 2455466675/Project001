using Navigation;
using UnityEngine;

namespace Game.UI
{
    [GroupProxy(NavigationGroupDefine.Test_Group_1)]
    public class Test_Group_1_Proxy : NavigationGroupProxy
    {
        public override void InFocus(NavigationGroupView group, bool isRefocus)
        {
            base.InFocus(group, isRefocus);
            Debug.Log("Test_Group_1_Proxy");
        }
    }
}