using Game.GSystem;
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
            UpdateData(Game.System.BattleSystem.GetHeroes());
        }

        public override void OnDisable()
        {
            MLog.Log("Battle_Player_Unit_List_Proxy OnDisable");
        }

        public override void OnBindData(GameNavigationItem item)
        {
            if (item.TryGetData(out BattleUnit unit))
            {
                unit.Attach(item as BattleNavigationItem);
                MLog.Log("OnBindData", unit.BattleId);
            }
        }

        public override void OnUnbindData(GameNavigationItem item)
        {
            if (item.TryGetData(out BattleUnit unit))
            {
                MLog.Log("OnUnbindData", unit.BattleId);
            }
        }
    }
}
