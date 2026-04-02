using GameFramework.Core;
using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    public class NavigationListCammand : GameInputCammand
    {
        public NavigationDefine Define { get; private set; }
        private int[] defaultIndexs;

        public NavigationListCammand(NavigationDefine define, int[] defaultIndexs) : base()
        {
            this.Define = define;
            this.defaultIndexs = defaultIndexs;
        }

        protected override void OnPop()
        {
            var list = GetListEntity();
            list?.Exit();
        }

        protected override bool OnPush()
        {
            var list = GetListEntity();
            if (list == null)
            {
                return false;
            }
            else
            {
                return list.InFocus(false, defaultIndexs);
            }
        }

        protected override bool OnRise()
        {
            if (IsPopAll)
            {
                return true;
            }

            var list = GetListEntity();
            if (list == null)
            {
                return false;
            }
            else
            {
                return list.InFocus(true);
            }
        }

        protected override void OnSink()
        {
            var list = GetListEntity();
            list?.OutFocus();
        }

        protected override bool CheckLocked()
        {
            var list = GetListEntity();
            return list != null && list.CheckLocked();
        }

        protected override void OnInputAction(InputContext context)
        {
            var list = GetListEntity();
            InputActionDefine define = context.Input;
            switch (define)
            {
                case InputActionDefine.Move:
                case InputActionDefine.Move2:
                    float x = context.X;
                    float y = context.Y;
                    list?.Move(x, y);
                    break;
                case InputActionDefine.Submit:
                    list?.Submit();
                    break;
            }
        }

        private NavigationListEntity GetListEntity()
        {
            var entity = Game.GetSystem<UISystem>().GetNavigationListEntity(Define);
            return entity;
        }
    }
}