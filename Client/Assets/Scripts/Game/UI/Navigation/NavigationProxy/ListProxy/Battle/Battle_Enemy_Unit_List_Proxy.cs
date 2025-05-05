using Game.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    [ListProxy(NavigationListDefine.Battle_Enemy_Unit_List)]
    public class Battle_Enemy_Unit_List_Proxy : NavigationListProxy
    {
        public override void OnEnable()
        {
            MLog.Log("Battle_Enemy_Unit_List_Proxy OnEnable");
            UpdateData(Game.System.BattleSystem.GetEnemies());
        }

        public override void OnDisable()
        {
            MLog.Log("Battle_Enemy_Unit_List_Proxy OnDisable");
        }

        public override void OnBindData(GameNavigationItem item)
        {
            if (item.TryGetData(out BattleUnit unit)) 
            {
                if (item.TryGetView(out BattleActorView view)) 
                {
                    BattleActorComponent bac = unit.GetComponent<BattleActorComponent>();
                    if (bac != null) 
                    {
                        bac.SetNode(view.transform);

                        if (unit.State == BattleUnitState.Alive) 
                        {
                            bac.Refresh();
                        }                        
                    }
                }

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
