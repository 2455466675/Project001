using UnityEngine.InputSystem;

namespace GameFramework.Core
{
    public class PageActionWrapper : InputActionWrapper
    {
        private bool isPress;

        public override void Tick()
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
            float v = inputAction.ReadValue<float>();
            Notify(new InputContext() { Input = InputDefine.Page, Y = v });
        }
    }
}
