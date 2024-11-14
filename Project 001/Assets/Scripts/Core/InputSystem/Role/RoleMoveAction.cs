using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// ½ÇÉ«ÒÆ¶¯
    /// </summary>
	public class RoleMoveAction : InputActionWrapper
    {
        public RoleMoveAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void FixedUpdate()
        {
            Execute();      
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            GameCore.Input.PushAction(this);
            Execute();
        }

        public override void OnPerformed(InputAction.CallbackContext obj)
        {
        }

        public override void OnCanceled(InputAction.CallbackContext obj)
        {
            GameCore.Input.PopAction(this);
            GameCore.System.RoleSystem.Stop();
        }

        public override void Execute()
        {
            Vector2 v = inputAction.ReadValue<Vector2>();           
            GameCore.System.RoleSystem.Move(v);
        }
    }
}

