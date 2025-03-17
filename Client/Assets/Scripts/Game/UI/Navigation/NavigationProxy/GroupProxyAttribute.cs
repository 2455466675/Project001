using System;

namespace Game.UI
{
    public class GroupProxyAttribute : Attribute
    {
        public NavigationGroupDefine Define { get; private set; }

        public GroupProxyAttribute(NavigationGroupDefine define) 
        {
            Define = define;
        }
    }
}