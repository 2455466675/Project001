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
            GameCore.InputManager.EnableRoleAction();         
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

