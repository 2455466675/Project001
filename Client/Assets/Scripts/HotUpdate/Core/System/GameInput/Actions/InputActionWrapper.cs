using System;
using UnityEngine.InputSystem;

namespace GameFramework.Core
{
    public struct InputContext 
    {
        public InputActionDefine Input { get; set; }
        public bool BoolValue { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, x:{2}, y:{3}", Input.ToString(), BoolValue, X, Y);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InputActionWrapper
    {
        public event Action<InputContext> OnInput;

        protected InputAction inputAction;

        public void Initialize(InputAction inputAction)
        {
            this.inputAction = inputAction;
            inputAction.started += OnStarted;
            inputAction.performed += OnPerformed;
            inputAction.canceled += OnCanceled;
        }

        /// <summary>
        /// FixedUpdate
        /// </summary>
        /// <param name="fdt"></param>
        public virtual void Tick() 
        {
        }

        /// <summary>
        /// 触发时机：当输入动作首次被检测到时触发（例如按下按键、移动摇杆或触控开始）。
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnStarted(InputAction.CallbackContext obj)
        {
        }

        /// <summary>
        /// 触发时机：当输入动作满足完成条件时触发。具体行为取决于交互类型（Interaction
        /// 默认交互（无额外配置）：在输入达到阈值（如按钮按下）时立即触发。在OnStarted之后。
        /// Hold 交互：按住一定时间后触发。
        /// Tap 交互：快速点击时触发。
        /// Value 类型输入（如摇杆、鼠标移动）：每次输入值变化时触发（需设置为持续触发模式）。
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnPerformed(InputAction.CallbackContext obj)
        {
        }

        /// <summary>
        /// 触发时机：当输入动作被中断或结束时触发。
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnCanceled(InputAction.CallbackContext obj)
        {
        }

        protected void Trigger(InputContext context) 
        {
            OnInput?.Invoke(context);
        }
    }
}
