using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class GameInputSystem : MonoBehaviour, ICore
    {
        private MyInput inputActions;

        private IInputActionController controller;
        private List<IInputActionController> controllers;
        private List<InputActionWrapper> continuedActions = new List<InputActionWrapper>();

        public IEnumerator Init()
        {
            inputActions = new MyInput();
            inputActions.Enable();

            controllers = new List<IInputActionController>
            {
                new UIInputActionController(inputActions),
                new RoleInputActionController(inputActions),
                new FightInputActionController(inputActions),
                new GMInputActionController(inputActions),
            };

            foreach (var item in controllers)
            {
                if (item.Mode != InputMode.GM)
                {
                    item.Disable();
                }
            }

            yield return null;
        }

        /// <summary>
        /// 切换输入模式
        /// </summary>
        /// <param name="mode"></param>
        public void SwitchInputMode(InputMode mode)
        {
            foreach (var item in controllers) 
            {
                if (item.Mode != mode && item.Mode != InputMode.GM)
                {
                    item.Disable();
                }
            }

            if (controller != null && controller.Mode == mode) 
            {
                return;
            }

            controller = controllers.Find(c => c.Mode == mode);
            controller.Enable();
        }

        /// <summary>
        /// 添加一个持续性的输入
        /// </summary>
        /// <param name="action"></param>
        public void PushAction(InputActionWrapper action)
        {
            continuedActions.Add(action);
        }

        /// <summary>
        /// 移除一个持续性输入
        /// </summary>
        /// <param name="action"></param>
        public void PopAction(InputActionWrapper action) 
        {
            continuedActions.Remove(action);
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < continuedActions.Count; i++)
            {
                continuedActions[i].FixedUpdate();
            }
        }    
    }
}

