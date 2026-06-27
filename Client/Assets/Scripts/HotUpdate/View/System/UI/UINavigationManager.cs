using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    public class UINavigationManager
    {
        private UIControllerManager controllerManager;
        private NavigateCommand command;

        public void Init(UIControllerManager controllerManager)
        {
            this.controllerManager = controllerManager;
            command = new NavigateCommand();
        }

        public void Navigate(NavigationDefine id, int[] defaultIndexs, object content)
        {
            PanelDefine panelDefine = controllerManager.GetPanelDefine(id);
            if (panelDefine == PanelDefine.None)
            {
                return;
            }

            defaultIndexs ??= new int[] { 0 };
            command.Navigate(id, panelDefine, defaultIndexs, content);
            command.RegisterInputSystem();
        }

        public void CloseNavigate()
        {
            command.PopAll();
            command.RegisterInputSystem();
        }
    }
}
