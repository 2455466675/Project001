using GameFramework.Core;

namespace GameFramework.UI 
{
    public class NavigationListCammand : InputCammand
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
            var list = GetList();
            list?.Exit();
        }

        protected override bool OnPush()
        {
            var list = GetList();
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

            var list = GetList();
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
            var list = GetList();
            list?.OutFocus();
        }

        protected override bool CheckLocked()
        {
            var list = GetList();
            return list != null && list.CheckLocked();
        }

        protected override void OnInputAction(InputContext context)
        {
            var list = GetList();
            InputDefine inputType = context.Input;
            switch (inputType)
            {
                case InputDefine.Move:
                    float x = context.X;
                    float y = context.Y;
                    list?.Move(x, y);
                    break;
                case InputDefine.Submit:
                    list?.Submit();
                    break;
            }
        }

        private NavigationListComponent GetList()
        {
            var entity = Game.GetModule<UIManager>().GetListEntity(Define);
            return entity.GetComponent<NavigationListComponent>();
        }
    }
}