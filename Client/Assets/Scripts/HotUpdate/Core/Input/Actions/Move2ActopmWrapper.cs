using UnityEngine;
using UnityEngine.InputSystem;

namespace GameFramework.Core
{
    public class Move2ActopmWrapper : InputActionWrapper
    {
        private bool isPress;

        public override void Tick()
        {
            if (!isPress)
            {
                return;
            }
            Execute();
        }

        protected override void OnStarted(InputAction.CallbackContext obj)
        {
            Execute();
            isPress = true;
        }

        protected override void OnCanceled(InputAction.CallbackContext obj)
        {
            isPress = false;
            Execute();
        }

        private void Execute()
        {
            Vector2 v = inputAction.ReadValue<Vector2>();
            Notify(new InputContext() { Input = InputDefine.Move2, X = v.x, Y = v.y });
        }
    }
}