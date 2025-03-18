namespace Game.UI
{
    public class ListProxyAttribute : GameAttribute
    {
        public NavigationListDefine Define { get; private set; }

        public ListProxyAttribute(NavigationListDefine define)
        {
            Define = define;
        }
    }
}