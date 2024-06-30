
namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GameLaunchState : IGameState
    {
        public GameState State {get;}

        public GameLaunchState() 
        {
            State = GameState.LAUNCH;
        }

        public void OnEnter(GameState preGameState)
        {
            MLog.Log("ÓÎÏ·×´Ì¬£ºÆô¶¯");
            GameCore.ResourceManager.LoadScene("scene001", UnityEngine.SceneManagement.LoadSceneMode.Single);
            GameCore.StateController.SwitchState(GameState.LOGIN);
        }

        public void OnExit(GameState nextGameState)
        {

        }

        public void OnStay()
        {

        }
    }
}