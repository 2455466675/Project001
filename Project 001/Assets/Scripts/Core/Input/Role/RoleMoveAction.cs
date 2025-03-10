using Game.System;
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

        public override void Tick()
        {
            Execute();      
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            
            Execute();
            PushContinued();
        }

        public override void OnPerformed(InputAction.CallbackContext obj)
        {
        }

        public override void OnCanceled(InputAction.CallbackContext obj)
        {       
            PopContinued();
            GameWorld.Instance.GetComponent<SystemComponent>().PartyComponent.Move(Vector2.zero);
        }

        public override void Execute()
        {
            Vector2 v = inputAction.ReadValue<Vector2>();
            GameWorld.Instance.GetComponent<SystemComponent>().PartyComponent.Move(v);
        }
    }
}

