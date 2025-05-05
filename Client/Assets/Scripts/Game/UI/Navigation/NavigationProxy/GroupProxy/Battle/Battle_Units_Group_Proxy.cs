using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    [GroupProxy(NavigationGroupDefine.Battle_Units_Group)]
    public class Battle_Units_Group_Proxy : NavigationGroupProxy
    {
        public override void LoadGroup(NavigationGroupDefine define)
        {
            GameObject go = GameObject.FindGameObjectWithTag("BattleUnits");
            if (go == null)
            {
                MLog.Error("BattleUnits is null");
                return;
            }
            MLog.Log("Battle_Units_Group_Proxy LoadGroup");
            groupView = go.GetComponent<NavigationGroupView>();
        }

        public override void Show()
        {
            if (groupView == null)
            {
                return;
            }
            groupView.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            if (groupView == null)
            {
                return;
            }
            groupView.gameObject.SetActive(false);
        }
    }
}
