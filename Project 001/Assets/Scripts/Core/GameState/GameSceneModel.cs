namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class GameSceneModel : IGameModel
    {
        public GameModel Model => GameModel.SCENE;

        public void OnEnter()
        {
            GameCore.InputManager.DisableUIAction();
            GameCore.InputManager.EnableRoleAction();
            MLog.Log("GameSceneModel OnEnter");
            
        }

        public void OnExit()
        {
            GameCore.InputManager.DisableRoleAction();            
        }

        public void OnStay()
        {

        }
    }
}

