using System;
using UnityEngine.InputSystem;

namespace GameFramework.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class LeftShiftActionWrapper : InputActionWrapper
    {
        protected override void OnStarted(InputAction.CallbackContext obj)
        {
            Notify(new InputContext() { Input = InputDefine.LeftShift, BoolValue = true });
        }

        protected override void OnCanceled(InputAction.CallbackContext obj)
        {
            Notify(new InputContext() { Input = InputDefine.LeftShift, BoolValue = false });
        }
    }
}
