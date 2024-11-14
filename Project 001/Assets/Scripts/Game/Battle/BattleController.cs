using Game.UI;
using Navigation;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattleController : MonoBehaviour
	{
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                StaticNavigationList list = NavigationList.GetNavigationList(ListName.BattleEnemyList) as StaticNavigationList;
                BattlePointItem fightPoint = list.GetItem(4) as BattlePointItem;

                fightPoint.actorLoader.actor.PlayAction("HitPopText");
            }

        }

        public void OnClickEnemyList(NavigationList list)
        {
            MLog.Log("OnClickEnemyList");
            GameCore.System.BattleSystem.ActionDetermine(list);
        }
    }
}

