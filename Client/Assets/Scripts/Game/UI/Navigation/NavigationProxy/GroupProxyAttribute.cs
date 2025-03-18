namespace Game.UI
{
    public class GroupProxyAttribute : GameAttribute
    {
        public NavigationGroupDefine Define { get; private set; }

        public GroupProxyAttribute(NavigationGroupDefine define) 
        {
            Define = define;
        }
    }
}