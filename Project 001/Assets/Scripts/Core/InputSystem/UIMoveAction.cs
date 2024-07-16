using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core
{
    public class UIMoveAction : InputActionBase
    {
        private float t;
        public UIMoveAction(InputAction inputAction) : base(inputAction)
        {
        }

        public override void Update()
        {
            if (t <= 0f)
            {
                Execute();
                t = 0.2f;
            }
            else
            {
                t -= Time.deltaTime;
            }
        }

        public override void OnStarted(InputAction.CallbackContext obj)
        {
            GameCore.InputManager.PushAction(this);
            Execute();
            t = 0.2f;
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
            Vector2 v = inputAction.ReadValue<Vector2>();
            if (v.x != 0)
            {
                GameCore.UI.TestH(v.x);
            }
            if (v.y != 0)
            {
                GameCore.UI.TestV(v.y);
            }
        }
    }
}

