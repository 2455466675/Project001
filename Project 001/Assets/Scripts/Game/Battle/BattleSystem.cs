using Game.Cfg;
using MVC;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 战斗系统
    /// </summary>
	public class BattleSystem : DataProxy, IGameSystem
    {
        private List<BattleRole> playerRoles;
        private List<BattleRole> enemyRoles;

        private BattleRoundController roundController;

        public BattleSystem(DataContainer container) : base(container)
        {
            roundController = new BattleRoundController();

            DataCollection playerPointList = CreateCollection("PlayerList");
            DataCollection enemyPointList = CreateCollection("EnemyList");

            playerRoles = new List<BattleRole>();
            enemyRoles = new List<BattleRole>();

            for (int i = 0; i < 9; i++)
            {
                DataContainer item = enemyPointList.Append();
                BattleRole role = new BattleEnemyRole(i, item);
                enemyRoles.Add(role);
            }

            for (int i = 0; i < 4; i++)
            {
                DataContainer item = playerPointList.Append();
                BattleRole role = new BattlePlayerRole(i, item);
                playerRoles.Add(role);
            }

            DataCollection actionList = CreateCollection("ActionList");

            List<BattleRoleActionCfg> cfgList = GameCore.Cfg.FindAll<BattleRoleActionCfg>();
            for (int i = 0; i < cfgList.Count; i++)
            {
                BattleRoleActionCfg cfg = cfgList[i];
                DataContainer item = actionList.Append();
                item.SetBaseValue("id", cfg.Id);
                item.SetBaseValue("name", cfg.Name);
            }
            
            DataCollection actionList2 = CreateCollection("ActionList2");
            for (int i = 0; i < 20; i++)
            {
                DataContainer item = actionList2.Append();
                item.SetBaseValue("id", i);
                item.SetBaseValue("name", $"Action2_{i}");
            }
        }

        /// <summary>
        /// 进入战斗
        /// </summary>
        public void Enter()
        {
            //GameCore.Scene.LoadSceneAsync("BattleScene", UnityEngine.SceneManagement.LoadSceneMode.Single, Loading, Entered);
        }

        public void ActionDetermine(object data)
        {
            roundController.ActionDetermine(data);
        }

        private void Loading(AsyncOperation operation)
        {
            MLog.Log("战斗场景加载中");
        }

        private void Entered(SceneInfo sceneInfo)
        {
            MLog.Log("进入战斗场景完毕");

            GameCore.Scene.characterContainer.gameObject.SetActive(false);

            foreach (var character in enemyRoles)
            {
                character.Reset();
            }

            foreach (var character in playerRoles)
            {
                character.Reset();
            }

            List<BattleRole> roles = new List<BattleRole>();

            int[] ids = new int[9]
            {
                300001, 0, 300003, 0, 300002, 0, 300004, 0, 300005,
            };

            for (int i = 0; i < ids.Length; i++)
            {
                enemyRoles[i].UpdateCfg(ids[i]);
                if (ids[i] > 0)
                {
                    roles.Add(enemyRoles[i]);
                }
            }

            int[] ids2 = new int[4]
            {
                1001, 0, 1002, 0,
            };

            for (int i = 0; i < ids2.Length; i++)
            {
                playerRoles[i].UpdateCfg(ids2[i]);
                if (ids2[i] > 0)
                {
                    roles.Add(playerRoles[i]);
                }

                if (playerRoles[i].Actor != null)
                {
                    playerRoles[i].Actor.PlayAction("battle_idle");
                }
            }

            roundController.StartFight(roles);
        }
    }
}

