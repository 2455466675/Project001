using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// ½ÇÉ«¼ÓËÙ
    /// </summary>
	public class RoleAddSpeedAction : InputActionWrapper
    {
        public RoleAddSpeedAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            GameCore.System.PartySystem.Run(true);
        }

        public override void OnCanceled(InputAction.CallbackContext obj)
        {
            GameCore.System.PartySystem.Run(false);
        }

        public override void Execute()
        {
            
        }
    }
}

