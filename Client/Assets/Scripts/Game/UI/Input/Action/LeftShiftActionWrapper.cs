using System;
using UnityEngine.InputSystem;

namespace Game.UI.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class LeftShiftActionWrapper : InputActionWrapper
    {
        public event Action<bool> ActionEvent;

        protected override void OnStarted(InputAction.CallbackContext obj)
        {
            ActionEvent?.Invoke(true);
        }

        protected override void OnCanceled(InputAction.CallbackContext obj)
        {
            ActionEvent?.Invoke(false);
        }
    }
}
