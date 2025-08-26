using UnityEngine.InputSystem;

namespace GameFramework.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class M_KeyboardActionWrapper : InputActionWrapper
    {
        protected override void OnStarted(InputAction.CallbackContext obj)
        {
            Trigger(new InputContext() { Input = InputDefine.M_Keyboard });
        }
    }
}
