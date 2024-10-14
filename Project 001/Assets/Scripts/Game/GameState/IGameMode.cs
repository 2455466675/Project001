namespace Game.Core
{
    public enum GameMode
    {
        SCENE = 1,
        UI = 2,
    }
    /// <summary>
    /// 
    /// </summary>
	public interface IGameMode
    {
        GameMode Mode { get; }

        void OnEnter(ModeArg arg);
        void OnStay();
        void OnExit();
    }

    public class ModeArg
    {

    }
}

