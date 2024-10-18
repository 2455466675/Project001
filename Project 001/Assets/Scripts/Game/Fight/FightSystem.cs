using MVC;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class FightSystem : DataProxy, IGameSystem
    {
        private DataCollection playerPointList;
        private DataCollection enemyPointList;

        public FightSystem(DataContainer container) : base(container)
        {
            playerPointList = CreateCollection("PlayerList");
            enemyPointList = CreateCollection("EnemyList");

            for (int i = 0; i < 9; i++)
            {
                DataContainer pointItem = enemyPointList.Append();
                pointItem.SetBaseValue("id", i);
                pointItem.SetBaseValue("valid", true);
            }

            for (int i = 0; i < 4; i++)
            {
                DataContainer pointItem = playerPointList.Append();
                pointItem.SetBaseValue("id", i);
                pointItem.SetBaseValue("valid", true);
            }
        }

        /// <summary>
        /// 进入战斗
        /// </summary>
        public void Enter()
        {
            GameCore.Scene.LoadSceneAsync("FightScene", UnityEngine.SceneManagement.LoadSceneMode.Single, Loading, Entered);
        }

        private void Loading(AsyncOperation operation)
        {
            MLog.Log("战斗场景加载中");
        }

        private void Entered(SceneInfo sceneInfo)
        {
            MLog.Log("进入战斗场景完毕");
            GameCore.UI.Exit();
            GameCore.UI.Enter(UI.ListName.FightPlayerList);
        }
    }
}

