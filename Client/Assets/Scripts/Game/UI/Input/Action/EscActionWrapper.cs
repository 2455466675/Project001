using System;
using UnityEngine.InputSystem;

namespace Game.UI.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class EscActionWrapper : InputActionWrapper
    {
        public event Action ActionEvent;

        protected override void OnStarted(InputAction.CallbackContext obj)
        {
            ActionEvent?.Invoke();
        }
    }
}
