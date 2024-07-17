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
            GameCore.InputManager.DisableRoleAction();
            GameCore.InputManager.EnableUIAction();
            GameCore.UI.OpenWin(100001); //引导界面
        }

        public void OnExit()
        {
            GameCore.InputManager.DisableUIAction();
            GameCore.UI.ExitUI();
        }

        public void OnStay()
        {
           
        }
    }
}

