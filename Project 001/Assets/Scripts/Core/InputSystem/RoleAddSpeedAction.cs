using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class RoleAddSpeedAction : InputActionBase
    {
        public RoleAddSpeedAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            GameCore.system.RoleSystem.isRun = true;
        }

        public override void OnCanceled(InputAction.CallbackContext obj)
        {
            GameCore.system.RoleSystem.isRun = false;
        }

        public override void Execute()
        {
            
        }
    }
}

