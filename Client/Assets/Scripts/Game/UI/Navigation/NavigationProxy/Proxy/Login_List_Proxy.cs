namespace Game.UI
{
    [ListProxy(NavigationListDefine.Login_List)]
    public class Login_List_Proxy : NavigationListProxy
    {
        public override bool IsLocked()
        {
            return true;
        }
    }
}