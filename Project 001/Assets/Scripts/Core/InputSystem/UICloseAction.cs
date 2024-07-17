using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class UICloseAction : InputActionBase
    {
        public UICloseAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {          
            Execute();
        }

        public override void Execute()
        {
            GameCore.StateController.SwitchModel(GameModel.SCENE);
        }
    }
}

