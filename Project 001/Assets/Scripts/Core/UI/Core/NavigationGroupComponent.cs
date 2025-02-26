using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupComponent : EC.Component
    {
        public NavigationGroup group;

        public void Init(NavigationGroup group) 
        {
            this.group = group;
        }

        public void OnShow() 
        {
            group.OnShow();
        }

        public void OnHide() 
        {
            group.OnHide();
        }

        public void OnInFocus()
        {
            group.OnInFocus();
        }

        public void OnOutFocus()
        {
            group.OnOutFocus();
        }

        public void OnMove(Vector2 dir)
        {
            group.OnMove(dir);
        }

        public void OnSubmit()
        {
            group.OnSubmit();
        }

        protected override void OnDestroy()
        {
            group = null;
        }
    }
}
