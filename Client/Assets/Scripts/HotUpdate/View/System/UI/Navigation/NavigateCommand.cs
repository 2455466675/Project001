using GameFramework.Core;
using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    public class NavigateCommand : GameInputCommand
    {
        private bool isDirty;

        public void Navigate(NavigationDefine navigationDefine, PanelDefine panelDefine, int[] defaultIndexs)
        {
            PanelCommand panelCommand;

            if (TryPeek(out PanelCommand command) && command.Define == panelDefine)
            {
                panelCommand = command;
            }
            else
            {
                panelCommand = new PanelCommand(panelDefine);
                Push(panelCommand);
            }

            if (panelCommand.TryPeek(out NavigationListCommand subCommand) && subCommand.Define == navigationDefine)
            {
                return;
            }

            NavigationListCommand listCommand = new NavigationListCommand(navigationDefine, defaultIndexs);
            panelCommand.Push(listCommand);
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
