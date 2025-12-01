using UnityEngine.InputSystem;

namespace GameFramework.Core
{
    public class GMActionWrapper : InputActionWrapper
    {
        protected override void OnStarted(InputAction.CallbackContext obj)
        {           
            Notify(new InputContext() { Input = InputDefine.GM });
        }
    }
}