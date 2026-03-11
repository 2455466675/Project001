using GameFramework.Core;
using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    public class NavigateCammand : GameInputCammand
    {
        private bool isDirty;

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
            InputActionDefine define = context.Input;
            switch (define)
            {
                case InputActionDefine.Cancel:
                    Pop();
                    break;
                case InputActionDefine.Esc:
                    PopAll();
                    break;
            }

            RegisterInputSystem();
        }

        public void RegisterInputSystem()
        {
            if (isDirty)
            {
                if (Count == 0)
                {
                    Game.GetSystem<GameInputSystem>().PopInputHandler();
                    isDirty = false;
                }
            }
            else
            {
                if (Count > 0)
                {
                    Game.GetSystem<GameInputSystem>().PushInputHandler(this);
                    isDirty = true;
                }
            }
        }
    }
}
