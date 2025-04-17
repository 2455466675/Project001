namespace Game.UI
{
    [ListProxy(NavigationListDefine.Overview_Menu_List)]
    public class Overview_Menu_List_Proxy : NavigationListProxy
    {
        public override void OnEnable()
        {
            listView.UpdateData(Game.System.OverviewSystem.GetMenus());
        }

        public override void OnSubmit(GameNavigationItem item)
        {
            MLog.Log("Overview_Menu_List_Proxy OnSubmit");
        }

        public override void OutFocus()
        {
            base.OutFocus();
        }
    }
}