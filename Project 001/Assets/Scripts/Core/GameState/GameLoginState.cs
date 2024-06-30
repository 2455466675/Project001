
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
            GameCore.UI.OpenWinCommond(100002, false); //µÇÂ¼½çÃæ
        }

        public void OnExit(GameState nextGameState)
        {

        }

        public void OnStay()
        {

        }
    }
}