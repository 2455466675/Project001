namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public enum GameState
    {
        None = 0,
        /// <summary>
        /// Æô¶¯
        /// </summary>
        LAUNCH = 1,
        /// <summary>
        /// µÇÂ¼
        /// </summary>
        LOGIN = 2,
        /// <summary>
        /// ÔËÐÐ
        /// </summary>
        PLAYING = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IGameState
    {
        GameState State {get;}

        void OnEnter(GameState preGameState);
        void OnStay();
        void OnExit(GameState nextGameState);
    }
}