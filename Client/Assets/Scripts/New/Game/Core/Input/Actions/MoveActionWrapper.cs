using UnityEngine;
using UnityEngine.InputSystem;

namespace GameFramework.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class MoveActionWrapper : InputActionWrapper
    {
        private bool isPress;

        public override void Tick(float fdt)
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
            Trigger(new InputContext() { Input = InputDefine.Move, X = v.x, Y = v.y });
        }
    }
}
