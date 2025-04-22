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
            UpdateData(Game.System.BackpackSystem.GetCompartments());
        }

        public override void OnSelect(GameNavigationItem item)
        {
            if (item.TryGetData(out BackpackCompartment compartment)) 
            {
               Game.System.BackpackSystem.OnSelectCompartment(compartment);
            }
        }

        public override void OnRefresh(GameNavigationItem item)
        {
            if (item.TryGetData(out BackpackCompartment compartment))
            {
                if (item.TryGetView(out TextView view)) 
                {
                    view.SetTextById(compartment.Cfg.Name);
                }
            }
        }

        public override void OnSubmit(GameNavigationItem item)
        {
            Game.UI.Navigate(NavigationListDefine.Backpack_Item_List);
        }
    }
}
