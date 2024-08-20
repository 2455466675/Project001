using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core
{
    public class UIMoveAction : InputActionBase
    {
        private float intervalTime = 0.15f;
        private float t;
        public UIMoveAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void Update()
        {
            if (t <= 0f)
            {
                Execute();
                t = intervalTime;
            }
            else
            {
                t -= Time.fixedDeltaTime;
            }
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            GameCore.InputManager.PushAction(this);
            Execute();
            t = intervalTime;
        }

        public override void OnPerformed(InputAction.CallbackContext obj)
        {
        }

        public override void OnCanceled(InputAction.CallbackContext obj)
        {
            GameCore.InputManager.PopAction(this);
        }

        public override void Execute()
        {
            GameCore.UI.Move(inputAction.ReadValue<Vector2>());            
        }
    }
}

