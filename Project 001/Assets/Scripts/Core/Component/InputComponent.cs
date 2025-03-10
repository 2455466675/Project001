using EC;
using System.Collections;
using System.Collections.Generic;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class InputComponent : Component, IInitializable, IUpdate
    {
        private MyInput gameInput;

        private IInputActionController controller;

        /// <summary>
        /// 各种输入场景控制器
        /// </summary>
        private List<IInputActionController> controllers;

        /// <summary>
        /// 持续性的输入行为
        /// </summary>
        private List<InputActionWrapper> continuedActions = new List<InputActionWrapper>();

        private Stack<IInputActionController> controllerStack;

        public IEnumerator Init(GameInitCfg intCfg)
        {
            controllerStack = new Stack<IInputActionController>();

            gameInput = new MyInput();
            gameInput.Enable();

            controllers = new List<IInputActionController>
            {
                new UIInputActionController(gameInput),
                new RoleInputActionController(gameInput),
                new FightInputActionController(gameInput),
                new GMInputActionController(gameInput),
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
        /// 将一个输入模式转到激活
        /// </summary>
        /// <param name="mode"></param>
        public void PushInputMode(InputMode mode) 
        {
            if (controllerStack.TryPeek(out controller)) 
            {
                if (controller.Mode != mode) 
                {
                    controller.Disable();
                }
                else
                {
                    return;
                }
            }

            controller = controllers.Find(c => c.Mode == mode);
            controller.Enable();
            controllerStack.Push(controller);
        }

        /// <summary>
        /// 退出最近一次添加的输入模式
        /// </summary>
        /// <param name="mode"></param>
        public void PopInputMode(InputMode mode)
        {
            if (controllerStack.TryPeek(out controller))
            {
                if (controller.Mode != mode) 
                {
                    return;
                }
            }

            if (controllerStack.TryPop(out controller))
            {
                controller.Disable();
            }
            if (controllerStack.TryPeek(out controller))
            {
                controller.Enable();
            }
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

        public void Update(float dt)
        {
            for (int i = 0; i < continuedActions.Count; i++)
            {
                continuedActions[i].Tick();
            }
        }
    }
}
