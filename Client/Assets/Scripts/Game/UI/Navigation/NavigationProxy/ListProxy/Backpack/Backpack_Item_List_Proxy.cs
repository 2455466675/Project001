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

        private void OnSelectBackpackMenuChanged(OnSelectBackpackCompartmentArg arg)
        {
            listView.UpdateData(arg.compartment.GetItems());
        }
    }
}
