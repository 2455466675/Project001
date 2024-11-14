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
        private List<BattleRole> playerCharacters;
        private List<BattleRole> enemyCharacters;

        private BattleRoundController roundController;

        public BattleSystem(DataContainer container) : base(container)
        {
            roundController = new BattleRoundController();

            DataCollection playerPointList = CreateCollection("PlayerList");
            DataCollection enemyPointList = CreateCollection("EnemyList");

            playerCharacters = new List<BattleRole>();
            enemyCharacters = new List<BattleRole>();

            for (int i = 0; i < 9; i++)
            {
                DataContainer item = enemyPointList.Append();
                BattleRole character = new BattleEnemyRole(i, item);
                enemyCharacters.Add(character);
            }

            for (int i = 0; i < 4; i++)
            {
                DataContainer item = playerPointList.Append();
                BattleRole character = new BattlePlayerRole(i, item);
                playerCharacters.Add(character);
            }

            DataCollection actionList = CreateCollection("ActionList");
            for (int i = 0; i < 20; i++)
            {
                DataContainer item = actionList.Append();
                item.SetBaseValue("id", i);
                item.SetBaseValue("name", $"Action_{i}");
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
            GameCore.Scene.LoadSceneAsync("BattleScene", UnityEngine.SceneManagement.LoadSceneMode.Single, Loading, Entered);
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

            foreach (var character in enemyCharacters)
            {
                character.Reset();
            }

            foreach (var character in playerCharacters)
            {
                character.Reset();
            }

            List<BattleRole> characters = new List<BattleRole>();

            int[] ids = new int[9]
            {
                300001, 0, 300003, 0, 300002, 0, 300004, 0, 300005,
            };

            for (int i = 0; i < ids.Length; i++)
            {
                enemyCharacters[i].UpdateCfg(ids[i]);
                if (ids[i] > 0)
                {
                    characters.Add(enemyCharacters[i]);
                }
            }

            int[] ids2 = new int[4]
            {
                1001, 0, 1002, 0,
            };

            for (int i = 0; i < ids2.Length; i++)
            {
                playerCharacters[i].UpdateCfg(ids2[i]);
                if (ids2[i] > 0)
                {
                    characters.Add(playerCharacters[i]);
                }

                if (playerCharacters[i].Actor != null)
                {
                    playerCharacters[i].Actor.PlayAction("battle_idle");
                }
            }

            roundController.StartFight(characters);
        }
    }
}

