using Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TestController : ViewController
    {
        protected override void Register()
        {
            FluidListView view = GetView<FluidListView>("FluidNavigationList");
            if (view != null)
            {
                //List<object> list = new List<object>();
                //for (int i = 0; i < 32; i++)
                //{
                //    list.Add(i);
                //}
                //view.UpdateData(list.ToArray());
            }
        }

        public void OnClickTestGroup1(NavigationItem item) 
        {
            //GameWorld.Root.GetComponent<UIComponent>().Navigate(NavigationListDefine.Test_List_2);
        }
    }
}
