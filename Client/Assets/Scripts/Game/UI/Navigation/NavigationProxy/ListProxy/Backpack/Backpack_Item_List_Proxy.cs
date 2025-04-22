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
            Game.Event.Register<OnSelectBackpackCompartmentArg>(OnSelectBackpackMenuChanged);
        }

        public override void OnDisable() 
        {
            Game.Event.Unregister<OnSelectBackpackCompartmentArg>(OnSelectBackpackMenuChanged);
        }

        public override void OnRefresh(GameNavigationItem item)
        {
            if (item.TryGetData(out BackpackItem backpackItem)) 
            {
                if (item.TryGetView(out TextView view)) 
                {
                    string name = Game.Config.GetTextById(backpackItem.Item.Config.Name);
                    view.SetTextByStr($"{name} X {backpackItem.Item.Count}");
                }
            }
        }

        public override void OnSubmit(GameNavigationItem item)
        {
            if (item.TryGetData(out BackpackItem backpackItem))
            {
                InventoryItemData2[] items = new InventoryItemData2[1];

                items[0] = new InventoryItemData2()
                {
                    uid = backpackItem.Item.Uid,
                    count = 1
                };
                Game.System.InventorySystem.Decrement(items);
            }
        }

        private void OnSelectBackpackMenuChanged(OnSelectBackpackCompartmentArg arg)
        {
            UpdateData(arg.compartment.GetItems());
        }
    }
}
