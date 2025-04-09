using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.UI.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class MoveActionWrapper : InputActionWrapper
    {
        public event Action<float, float> ActionEvent;

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
            ActionEvent?.Invoke(v.x, v.y);
        }
    }
}
