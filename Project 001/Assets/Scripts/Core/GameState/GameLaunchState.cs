namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GameLaunchState : IGameState
    {
        public GameState State => GameState.LAUNCH;

        public void OnEnter(GameState preGameState)
        {
            MLog.Log("ÓÎÏ·×´Ì¬£ºÆô¶¯");
            GameWorld.Instance.GetComponent<GSMComponent>().SwitchState(GameState.LOGIN);
        }

        public void OnExit(GameState nextGameState)
        {

        }
    }
}