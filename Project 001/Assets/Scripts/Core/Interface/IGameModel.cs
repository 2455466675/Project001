namespace Game.Core
{
    public enum GameModel
    {
        SCENE = 1,
        UI = 2,
    }
    /// <summary>
    /// 
    /// </summary>
	public interface IGameModel
	{
        GameModel Model { get; }

        void OnEnter();
        void OnStay(); 
        void OnExit();
    }
}

