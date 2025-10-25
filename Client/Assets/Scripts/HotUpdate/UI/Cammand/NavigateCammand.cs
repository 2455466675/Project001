using GameFramework.Core;

namespace GameFramework.UI
{
    public class NavigateCammand : InputCammand
    {
        public void Navigate(NavigationDefine navigationDefine, PanelDefine panelDefine, int[] defaultIndexs)
        {
            PanelCammand panelCammand;

            if (TryPeek(out PanelCammand cammand) && cammand.Define == panelDefine)
            {
                panelCammand = cammand;
            }
            else
            {
                panelCammand = new PanelCammand(panelDefine);
                Push(panelCammand);
            }

            if (panelCammand.TryPeek(out NavigationListCammand subCammand) && subCammand.Define == navigationDefine)
            {
                return;
            }

            NavigationListCammand listCammand = new NavigationListCammand(navigationDefine, defaultIndexs);
            panelCammand.Push(listCammand);
        }

        protected override void OnInputAction(InputContext context)
        {
            InputDefine inputDefine = context.Input;
            switch (inputDefine)
            {
                case InputDefine.Cancel:
                    Pop();
                    break;
                case InputDefine.Esc:
                    PopAll();
                    break;
            }
        }
    }
}
