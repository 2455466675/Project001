namespace Game.UI
{
    [ListProxy(NavigationListDefine.GM_Menu_List)]
    public class GM_Menu_List_Proxy : NavigationListProxy
    {
        public override void OnEnable()
        {
            UpdateData(Game.GM.GetMenus());
        }

        public override void OnSelect(GameNavigationItem item)
        {
            if (item.TryGetData(out GM_Menu result)) 
            {
                Game.GM.Select(result);
            }
        }

        public override void OnSubmit(GameNavigationItem item)
        {
            Game.UI.Navigate(NavigationListDefine.GM_Item_List);
        }

        public override void OnRefresh(GameNavigationItem item)
        {
            if (item.TryGetData(out GM_Menu result))
            {
                if (item.TryGetView(out TextView view)) 
                {
                    view.SetTextByStr(result.Type.ToString());
                }
            }
        }
    }
}