namespace Game
{
    public enum GameStateDefine 
    {
        Undefined = 0,
        Login     = 1,
        Playing   = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class GameState
    {
        public abstract GameStateDefine Define { get; }
        public abstract void Enter();
        public abstract void Exit();        
    }
}
