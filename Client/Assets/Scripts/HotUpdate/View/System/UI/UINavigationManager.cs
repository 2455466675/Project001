using GameFramework.Core;
using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    public class UINavigationManager
    {
        private NavigateCammand cammand;

        public void Init()
        {
            cammand = new NavigateCammand();
        }

        public void Navigate(NavigationDefine id, int[] defaultIndexs)
        {
            PanelDefine panelDefine = Game.GetSystem<UISystem>().GetPanelDefine(id);
            if (panelDefine == PanelDefine.None)
            {
                return;
            }

            defaultIndexs ??= new int[] { 0 };
            cammand.Navigate(id, panelDefine, defaultIndexs);
            cammand.RegisterInputSystem();
        }

        public void CloseNavigate()
        {
            cammand.PopAll();
            cammand.RegisterInputSystem();
        }
    }
}
