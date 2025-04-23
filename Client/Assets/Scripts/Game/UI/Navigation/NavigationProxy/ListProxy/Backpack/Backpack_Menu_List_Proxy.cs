using Game.System;
using Navigation;
using System.Collections.Generic;

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

        public override void OnDisable()
        {
            Game.System.BackpackSystem.OnSelectCompartment(null);
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

        protected override INavigationItemData[] FilterData(INavigationItemData[] datas)
        {
            List<BackpackCompartment> compartments = new List<BackpackCompartment>();

            int newType = (int)BackpackCompartmentType.New;

            for (int i = 0; i < datas.Length; i++) 
            {
                BackpackCompartment compartment = datas[i] as BackpackCompartment;
                if (compartment.Cfg.Type == newType) 
                {
                    if (compartment.Count > 0) 
                    {
                        compartments.Add(compartment);                    
                    }
                }
                else
                {
                    compartments.Add(compartment);
                }
            }

            compartments.Sort((a, b) =>
            {
                return b.Cfg.Sort.CompareTo(a.Cfg.Sort);
            });

            return compartments.ToArray();
        }
    }
}
