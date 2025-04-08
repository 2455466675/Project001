using Navigation;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TestController2 : Controller
    {
        protected override void Register()
        {
            FluidListView view2 = GetView<FluidListView>("FluidNavigationList");

            if (view2 != null)
            {
                List<object> list = new List<object>();
                for (int i = 0; i < 20; i++)
                {
                    list.Add(i);
                }

                view2.UpdateData(list.ToArray());
            }

            FixedListView view3 = GetView<FixedListView>("FixedNavigationList");

            if (view3 != null)
            {
                view3.UpdateData(new object[]{ 1, 2, 3, 4, 5, 6, 7, 8 });
            }
        }

        public void OnClickTestGroup2(NavigationItem item)
        {
            //GameWorld.Root.GetComponent<UIComponent>().Navigate(NavigationListDefine.Test_List_3);
        }

        public void OnClickTestGroup3(NavigationItem item)
        {
            Debug.Log("OnClickTestGroup3");
        }
    }
}
