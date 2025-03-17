using System;
using UnityEngine.InputSystem;

namespace Game.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class MapActionWrapper : InputActionWrapper
    {
        public event Action ActionEvent;

        protected override void OnStarted(InputAction.CallbackContext obj)
        {
            ActionEvent?.Invoke();
        }
    }
}
