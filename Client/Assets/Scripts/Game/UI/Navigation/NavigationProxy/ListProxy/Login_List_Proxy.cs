using Game.System;

namespace Game.UI
{
    [ListProxy(NavigationListDefine.Login_List)]
    public class Login_List_Proxy : NavigationListProxy
    {
        public override bool IsLocked()
        {
            bool state = GameWorld.Root.GetComponent<SystemComponent>().LoginSystem.LockLoginGroup;
            return state;
        }

        public override void OnEnable()
        {
            LoginSystem loginSystem = GameWorld.Root.GetComponent<SystemComponent>().LoginSystem;
            loginSystem.LockLoginGroup = true;
            listView.UpdateData(loginSystem.GetOptions());
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