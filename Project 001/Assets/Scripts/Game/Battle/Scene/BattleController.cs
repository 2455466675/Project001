using Game.UI;
using UnityEngine;
using NavigationList = Navigation.NavigationList;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattleController : MonoBehaviour
	{
        [SerializeField]
        private StaticNavigationGroup playerGroup;
        [SerializeField]
        private StaticNavigationGroup enemyGroup;

        private void Awake()
        {
            playerGroup.Init();
            enemyGroup.Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                //BattlePointItem fightPoint = list.GetItem(4) as BattlePointItem;

                //fightPoint.actorLoader.actor.PlayAction("HitPopText");
            }

        }

        public void OnClickEnemyList(NavigationList list)
        {
            MLog.Log("OnClickEnemyList");
            GameCore.System.BattleSystem.ActionDetermine(list);
        }
    }
}

