using Game.UI;
using System;
using System.Reflection;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class UIComponent : ECS.Component
    {
        private InputController controller;

        public void Init() 
        {
            controller = new InputController();
        }

        public void InputAction(ActionContext context) 
        {
            controller.InputAction(context);
        }

        public void Navigate(NavigationListDefine list_ID, ModuleType moduleType = ModuleType.Panel) 
        {
            controller.Navigate(list_ID, moduleType);
        }
    }
}
