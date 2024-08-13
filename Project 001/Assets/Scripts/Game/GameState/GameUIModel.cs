namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class GameUIModel : IGameModel
    {
        public GameModel Model => GameModel.UI;

        public void OnEnter()
        {
            GameCore.InputManager.EnableUIAction();
        }

        public void OnExit()
        {
            GameCore.InputManager.DisableUIAction();
        }

        public void OnStay()
        {
           
        }
    }
}

