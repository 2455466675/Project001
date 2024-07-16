using Game.UI;
using System;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GamePlayingState : IGameState
    {
        public GameState State { get; }

        public GamePlayingState()
        {
            State = GameState.PLAYING;
        }

        public void OnEnter(GameState preGameState)
        {
            MLog.Log("游戏状态：运行");

            GameCore.Co.StartCoroutine(GameCore.ResourceManager.LoadSceneAsync("scene002", UnityEngine.SceneManagement.LoadSceneMode.Single, Fun1, Fun2));            
        }

        private void Fun2(SceneInfo info)
        {
            MLog.Log("加载场景结束");
            GameCore.StateController.SwitchModel(GameModel.SCENE);

            GameCore.system.RoleSystem.Init();
        }

        private void Fun1(AsyncOperation operation)
        {
            MLog.Log("加载中:", operation.progress, Time.frameCount);
        }

        public void OnExit(GameState nextGameState)
        {

        }

        public void OnStay()
        {

        }
    }
}