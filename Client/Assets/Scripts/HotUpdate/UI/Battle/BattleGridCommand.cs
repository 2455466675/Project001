using GameFramework.Core;
using GameFramework.Gameplay;

namespace GameFramework.UI
{
    public class BattleGridCommand : InputCommand
    {
        private NavigationController controller;
        private int[] defaultIndexs;

        public BattleGridCommand(NavigationController controller, int[] defaultIndexs)
        {
            this.controller = controller;
            this.defaultIndexs = defaultIndexs;
        }

        protected override void OnPop()
        {
            Game.GetSystem<BattleSystem>().GridManager.Wipe();
            controller?.Exit();
        }

        protected override bool OnPush()
        {
            if (controller == null)
            {
                return false;
            }
            else
            {
                return controller.InFocus(false, defaultIndexs);
            }
        }

        protected override bool OnRise()
        {
            return base.OnRise();
        }

        protected override void OnSink()
        {
            Game.GetSystem<BattleSystem>().GridManager.Wipe();
        }

        protected override bool CheckLocked()
        {
            return controller != null && controller.IsLocked;
        }

        protected override void OnInputAction(InputContext context)
        {
            InputDefine inputType = context.Input;
            switch (inputType)
            {
                case InputDefine.Move:
                    float x = context.X;
                    float y = context.Y;
                    controller?.Move(x, y);
                    break;
                case InputDefine.Submit:
                    controller?.Submit();
                    break;
            }
        }
    }
}
