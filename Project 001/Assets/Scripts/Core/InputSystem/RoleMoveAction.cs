using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class RoleMoveAction : InputActionBase
    {
        public RoleMoveAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void Update()
        {
            Execute();      
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            GameCore.InputManager.PushAction(this);
            Execute();
        }

        public override void OnPerformed(InputAction.CallbackContext obj)
        {
        }

        public override void OnCanceled(InputAction.CallbackContext obj)
        {
            GameCore.InputManager.PopAction(this);
            GameCore.System.RoleSystem.Stop();
        }

        public override void Execute()
        {
            Vector2 v = inputAction.ReadValue<Vector2>();           
            GameCore.System.RoleSystem.Move(v.x, v.y);
        }
    }
}

