using Game.System;

namespace Game.UI
{
    [ListProxy(NavigationListDefine.Login_List)]
    public class Login_List_Proxy : NavigationListProxy
    {
        public override bool IsLocked()
        {
            bool state = Game.System.LoginSystem.LockLoginGroup;
            return state;
        }

        public override void OnEnable()
        {
            Game.System.LoginSystem.LockLoginGroup = true;
            UpdateData(Game.System.LoginSystem.GetOptions());
        }

        public override void OnSubmit(GameNavigationItem item)
        {
            if (item.TryGetData(out LoginOption option)) 
            {
                option.Execute();
            }
        }
    }
}