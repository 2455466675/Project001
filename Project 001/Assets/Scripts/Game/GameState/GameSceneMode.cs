namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class GameSceneMode : IGameMode
    {
        public GameMode Mode => GameMode.SCENE;

        public void OnEnter(ModeArg arg)
        {
            GameCore.InputSys.SwitchInputMode(InputMode.Role);         
        }

        public void OnExit()
        {       
        }

        public void OnStay()
        {

        }
    }
}

