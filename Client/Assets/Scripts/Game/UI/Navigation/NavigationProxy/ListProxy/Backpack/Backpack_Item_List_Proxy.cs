using Game.System;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    [ListProxy(NavigationListDefine.Backpack_Item_List)]
    public class Backpack_Item_List_Proxy : NavigationListProxy
    {
        public override void OnEnable()
        {
            Game.Event.Register<SelectBackpackMenu>(OnSelectBackpackMenuChanged);
        }

        public override void OnDisable() 
        {
            Game.Event.Unregister<SelectBackpackMenu>(OnSelectBackpackMenuChanged);
        }

        public override void OnRefresh(GameNavigationItem item)
        {
            if (item.TryGetData(out InventoryItem inventoryItem)) 
            {
                if (item.TryGetView(out TextView view)) 
                {
                    string name = Game.Config.GetTextById(inventoryItem.Config.Name);
                    view.SetTextByStr($"{name} X {inventoryItem.Count}");
                }
            }
        }

        private void OnSelectBackpackMenuChanged(SelectBackpackMenu menu) 
        {
            listView.UpdateData(menu.backpack.GetItems());
        }
    }
}
