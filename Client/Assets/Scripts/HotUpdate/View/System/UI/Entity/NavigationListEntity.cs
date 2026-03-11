using GameFramework.Utility.GameDefine;

namespace GameFramework.View.UI
{
    public class NavigationListEntity
    {
        private NavigationView view;
        private INavigationController controller;

        public void Init(NavigationView view, NavigationDefine id)
        {
            this.view = view;
            controller = Game.GetSystem<UISystem>().GetNavigationController(id);
            if (controller == null)
            {
                MDebug.Error("NavigationController is null : ", id.ToString());
            }
        }

        public void Destroy()
        {
            view = null;
            controller = null;
        }


        public void Show()
        {
            controller?.Show(view);
        }

        public void Hide()
        {
            controller?.Hide();
        }

        public void Move(float h, float v)
        {
            controller?.Move(h, v);
        }

        public void Submit()
        {
            controller?.Submit();
        }

        public bool InFocus(bool isRefocus, int[] indexs = null)
        {
            if (controller == null) return false;
            return controller.InFocus(isRefocus, indexs);
        }

        public void OutFocus()
        {
            controller?.OutFocus();
        }

        public void Exit()
        {
            controller?.Exit();
        }

        public bool CheckLocked()
        {
            if (controller == null) return false;
            return controller.CheckIsLocked();
        }
    }
}