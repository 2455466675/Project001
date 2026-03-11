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
            Trigger(new InputContext() { Input = InputActionDefine.LeftShift, BoolValue = true });
        }

        protected override void OnCanceled(InputAction.CallbackContext obj)
        {
            Trigger(new InputContext() { Input = InputActionDefine.LeftShift, BoolValue = false });
        }
    }
}
