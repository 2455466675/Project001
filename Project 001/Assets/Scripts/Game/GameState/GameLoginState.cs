
namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GameLoginState : IGameState
    {
        public GameState State { get; }

        public GameLoginState()
        {
            State = GameState.LOGIN;
        }

        public void OnEnter(GameState preGameState)
        {
            MLog.Log("ÓÎÏ·×´Ì¬£ºµÇÂ¼");
            GameCore.UI.Enter(UI.ListName.Login);
        }

        public void OnExit(GameState nextGameState)
        {

        }

        public void OnStay()
        {

        }
    }
}