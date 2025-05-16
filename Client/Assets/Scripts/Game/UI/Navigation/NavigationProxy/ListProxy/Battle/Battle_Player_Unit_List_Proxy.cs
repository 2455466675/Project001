using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    [ListProxy(NavigationListDefine.Battle_Player_Unit_List)]
    public class Battle_Player_Unit_List_Proxy : NavigationListProxy
    {
        public override void OnEnable()
        {
            MLog.Log("Battle_Player_Unit_List_Proxy OnEnable");
        }

        public override void OnDisable()
        {
            MLog.Log("Battle_Player_Unit_List_Proxy OnDisable");
        }
    }
}
