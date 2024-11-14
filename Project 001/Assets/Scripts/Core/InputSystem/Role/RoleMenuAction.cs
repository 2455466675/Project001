using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// Ö÷½çÃæ
    /// </summary>
	public class RoleMenuAction : InputActionWrapper
    {
        public RoleMenuAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            Execute();
        }

        public override void Execute()
        {
            GameCore.UI.Enter(UI.ListName.OverviewMenu);
        }
    }
}

