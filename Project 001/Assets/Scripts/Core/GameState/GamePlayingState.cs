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
            MLog.Log("ÓÎÏ·×´Ì¬£ºÔËÐÐ");
        }

        public void OnExit(GameState nextGameState)
        {

        }

        public void OnStay()
        {

        }
    }
}