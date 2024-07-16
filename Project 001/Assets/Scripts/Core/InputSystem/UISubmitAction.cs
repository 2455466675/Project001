using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class UISubmitAction : InputActionBase
    {
        public UISubmitAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            Execute();
        }

        public override void Execute()
        {
            GameCore.UI.Submit();
        }
    }
}

