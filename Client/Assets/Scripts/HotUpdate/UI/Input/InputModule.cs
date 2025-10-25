using GameFramework.Core;

namespace GameFramework.UI
{
    public abstract class InputModule : IInputable
    {
        private readonly NavigateCammand navigateCammand;

        public abstract InputModuleType ModuleType { get; }

        public InputModule()
        {
            navigateCammand = new NavigateCammand();
        }

        public void EnterNavigate(NavigationDefine navigationDefine, PanelDefine panelDefine, int[] defaultIndexs)
        {
            navigateCammand.Navigate(navigationDefine, panelDefine, defaultIndexs);
        }

        public void ExitNavigate() 
        {
            navigateCammand.PopAll();
        }

        public void InputAction(InputContext context)
        {
            if (navigateCammand.Count > 0)
            {
                navigateCammand.InputAction(context);
            }
            else
            {
                OnInputAction(context);
            }
        }

        protected abstract void OnInputAction(InputContext context);
    }
}
