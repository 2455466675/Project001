using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class UICloseAction : InputActionWrapper
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
            GameCore.UI.Close();
        }
    }
}

