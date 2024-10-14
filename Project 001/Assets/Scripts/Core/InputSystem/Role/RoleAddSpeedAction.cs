using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class RoleAddSpeedAction : InputActionWrapper
    {
        public RoleAddSpeedAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            GameCore.System.RoleSystem.Run(true);
        }

        public override void OnCanceled(InputAction.CallbackContext obj)
        {
            GameCore.System.RoleSystem.Run(false);
        }

        public override void Execute()
        {
            
        }
    }
}

