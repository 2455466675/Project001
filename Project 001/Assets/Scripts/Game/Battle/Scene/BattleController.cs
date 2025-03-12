using EC;
using Game.Core;
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
                MLog.Log("ExitBattleScene");
                GameWorld.Instance.GetComponent<SystemComponent>().BattleComponent.Exit();
            }

        }

        public void OnClickEnemyList(NavigationList list)
        {
            MLog.Log("OnClickEnemyList");
        }
    }
}

