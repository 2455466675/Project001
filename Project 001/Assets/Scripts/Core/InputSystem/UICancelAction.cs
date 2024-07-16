using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class UICancelAction : InputActionBase
    {
        public UICancelAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            Execute();
        }

        public override void Execute()
        {
            GameCore.UI.UndoCommand();
        }
    }
}

