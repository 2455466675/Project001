using UnityEngine.InputSystem;

namespace Game.Core
{
    /// <summary>
    /// 输入行为装饰类
    /// </summary>
    public abstract class InputActionWrapper
    {
        protected InputAction inputAction;

        public InputActionWrapper(InputAction inputAction)
        {
            this.inputAction = inputAction;
            this.inputAction.started += OnStarted;
            this.inputAction.performed += OnPerformed;
            this.inputAction.canceled += OnCanceled;
        }

        public virtual void FixedUpdate()
        {
        }

        /// <summary>
        /// 当输入操作刚刚开始时触发。
        /// </summary>
        /// <param name="obj"></param>
        public virtual void OnStarted(InputAction.CallbackContext obj)
        {
        }

        /// <summary>
        /// 当输入操作执行时触发，具体表现取决于输入类型。对于按键输入，通常在按下时触发；对于持续输入（如移动、拖动），会在输入值变化时反复触发。
        /// </summary>
        /// <param name="obj"></param>
        public virtual void OnPerformed(InputAction.CallbackContext obj)
        {
        }

        /// <summary>
        /// 当输入操作被取消或结束时触发。通常在输入状态从激活转为未激活时调用。
        /// </summary>
        /// <param name="obj"></param>
        public virtual void OnCanceled(InputAction.CallbackContext obj)
        {
        }

        public abstract void Execute();
    }
}

