using UnityEngine.InputSystem;

namespace Game.Core
{
    public abstract class InputActionBase
    {
        protected InputAction inputAction;

        public InputActionBase(InputAction inputAction)
        {
            this.inputAction = inputAction;
            this.inputAction.started += OnStarted;
            this.inputAction.performed += OnPerformed;
            this.inputAction.canceled += OnCanceled;
        }

        public virtual void Update()
        {
        }

        public virtual void OnStarted(InputAction.CallbackContext obj)
        {
        }

        public virtual void OnPerformed(InputAction.CallbackContext obj)
        {
        }

        public virtual void OnCanceled(InputAction.CallbackContext obj)
        {
        }

        public abstract void Execute();
    }
}

