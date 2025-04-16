namespace Game.UI
{
    [ListProxy(NavigationListDefine.GM_Item_List)]
    public class GM_Item_List_Proxy : NavigationListProxy
    {
        public override void OnEnable()
        {
            Game.Event.Register<CurrentMenuChanged>(OnCurrentMenuChanged);
        }

        public override void OnDisable()
        {
            Game.Event.Unregister<CurrentMenuChanged>(OnCurrentMenuChanged);
        }

        public override void OnRefresh(GameNavigationItem item)
        {
            if (item.TryGetData(out GM_Item result)) 
            {
                if (item.TryGetView(out TextView view)) 
                {
                    view.SetTextByStr(result.Cfg.Name);
                }
            }
        }

        public override void OnSubmit(GameNavigationItem item)
        {
            if (item.TryGetData(out GM_Item result))
            {
                Game.GM.Submit(result);
            }
        }

        private void OnCurrentMenuChanged(CurrentMenuChanged arg) 
        {
            GM_Menu menu = arg.menu;
            if (menu == null) 
            {
                listView.UpdateData(new object[0]);
            }
            else
            {
                listView.UpdateData(menu.GetItems());
            }
        }
    }
}