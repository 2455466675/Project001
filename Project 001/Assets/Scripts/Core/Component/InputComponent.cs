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

        public IEnumerator Init(GameInitCfg intCfg)
        {
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

        public void Update(float dt)
        {
            for (int i = 0; i < continuedActions.Count; i++)
            {
                continuedActions[i].Tick();
            }
        }
    }
}
