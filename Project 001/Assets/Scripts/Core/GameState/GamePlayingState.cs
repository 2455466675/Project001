using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GamePlayingState : IGameState
    {
        public GameState State => GameState.PLAYING;

        public void OnEnter(GameState preGameState)
        {
            MLog.Log("游戏状态：运行");
            GameWorld.Instance.GetComponent<SceneComponent>().LoadSceneAsync("scene002", UnityEngine.SceneManagement.LoadSceneMode.Single, Fun1, Fun2);
        }

        public void OnExit(GameState nextGameState)
        {

        }

        private void Fun1(float progress)
        {
            MLog.Log("加载中:", progress, Time.frameCount);
        }

        private void Fun2(SceneEntity info)
        {
            GameWorld.Instance.GetComponent<UIComponent>().Close(true);
            GameWorld.Instance.GetComponent<InputComponent>().PushInputMode(InputMode.Role);
            MLog.Log("加载场景结束");
        }
    }
}