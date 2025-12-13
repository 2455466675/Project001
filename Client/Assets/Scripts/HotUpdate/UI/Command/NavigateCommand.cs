using GameFramework.Core;

namespace GameFramework.UI
{
    public class NavigateCommand : InputCommand
    {
        public void Navigate(NavigationDefine navigationDefine, PanelDefine panelDefine, int[] defaultIndexs)
        {
            PanelCommand panelCammand;

            if (TryPeek(out PanelCommand cammand) && cammand.Define == panelDefine)
            {
                panelCammand = cammand;
            }
            else
            {
                panelCammand = new PanelCommand(panelDefine);
                Push(panelCammand);
            }

            if (panelCammand.TryPeek(out NavigationListCommand subCammand) && subCammand.Define == navigationDefine)
            {
                return;
            }

            NavigationListCommand listCammand = new NavigationListCommand(navigationDefine, defaultIndexs);
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
