namespace Game.Core
{
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