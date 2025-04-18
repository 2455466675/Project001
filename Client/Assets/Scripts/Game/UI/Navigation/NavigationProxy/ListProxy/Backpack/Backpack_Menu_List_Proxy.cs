using Game.System;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    [ListProxy(NavigationListDefine.Backpack_Menu_List)]
    public class Backpack_Menu_List_Proxy : NavigationListProxy
    {
        public override void OnEnable()
        {
            listView.UpdateData(Game.System.InventorySystem.GetBackpacks());
        }

        public override void OnSelect(GameNavigationItem item)
        {
            if (item.TryGetData(out Backpack backpack)) 
            {
                Game.System.InventorySystem.OnSelectBackpack(backpack);
            }
        }

        public override void OnRefresh(GameNavigationItem item)
        {
            if (item.TryGetData(out Backpack backpack))
            {
                if (item.TryGetView(out TextView view)) 
                {
                    view.SetTextById(backpack.Cfg.Name);
                }
            }
        }
    }
}
