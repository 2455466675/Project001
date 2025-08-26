namespace GameFramework.UI 
{
    public class NavigationControllerAttribute : GameAttribute
    {
        public NavigationDefine Id { get; private set; }

        public NavigationControllerAttribute(NavigationDefine id) 
        { 
            Id = id;
        }
    }
}