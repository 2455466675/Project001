using GameFramework.Core;

namespace GameFramework.UI
{
    public abstract class InputModule : IInputable
    {
        public abstract InputModuleType ModuleType { get; }


        protected readonly NavigateCammand navigateCammand;

        private readonly GameCammand cammands;

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

        public void PushCammand(GameCammand cammand)
        {
            navigateCammand.Push(cammand);
        }

        public abstract void InputAction(InputContext context);
    }
}
