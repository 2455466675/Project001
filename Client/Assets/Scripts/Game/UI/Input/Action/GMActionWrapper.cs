using System;
using UnityEngine.InputSystem;

namespace Game.UI.Input
{
    public class GMActionWrapper : InputActionWrapper
    {
        public event Action ActionEvent;

        protected override void OnStarted(InputAction.CallbackContext obj)
        {
            ActionEvent?.Invoke();
        }
    }
}