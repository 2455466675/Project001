using System;

namespace GameFramework.UI 
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UIPanelControllerAttribute : GameAttribute
    {       
        public PanelDefine Id { get; private set; }

        public NavigationDefine[] Children { get; private set; }

        public UIPanelControllerAttribute(PanelDefine id, params NavigationDefine[] children)
        {
            Id = id;
            Children = children;
        }
    }
}
