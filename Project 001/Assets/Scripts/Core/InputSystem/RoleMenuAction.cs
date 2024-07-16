using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class RoleMenuAction : InputActionBase
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
            MLog.Log("RoleMenuAction Execute");
            GameCore.UI.OpenWin(100003);
        }
    }
}

