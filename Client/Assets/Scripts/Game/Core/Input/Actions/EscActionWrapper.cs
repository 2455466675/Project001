using UnityEngine.InputSystem;

namespace GameFramework.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class EscActionWrapper : InputActionWrapper
    {
        protected override void OnStarted(InputAction.CallbackContext obj)
        {
            Trigger(new InputContext() { Input = InputDefine.Esc });
        }
    }
}
