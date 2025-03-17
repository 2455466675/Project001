using System;

namespace Game.UI
{
    public class ListProxyAttribute : Attribute
    {
        public NavigationListDefine Define { get; private set; }

        public ListProxyAttribute(NavigationListDefine define)
        {
            Define = define;
        }
    }
}