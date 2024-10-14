namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class GameUIMode : IGameMode
    {
        public GameMode Mode => GameMode.UI;

        public void OnEnter(ModeArg arg)
        {
            GameCore.InputSys.SwitchInputMode(InputMode.UI);
        }

        public void OnExit()
        {
        }

        public void OnStay()
        {
           
        }
    }
}

