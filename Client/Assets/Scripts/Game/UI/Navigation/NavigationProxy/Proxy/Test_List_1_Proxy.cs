using UnityEngine;

namespace Game.UI
{
    [ListProxy(NavigationListDefine.Test_List_1)]
    public class Test_List_1_Proxy : NavigationListProxy
    {
        public override void OnEnable(NavigationListView list)
        {
            int count = Random.Range(5, 20);

            object[] data = new object[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = i;
            }
            list.UpdateData(data);
        }

        public override void OnRefresh(NavigationListView list, GameNavigationItem item)
        {
            TextView view = item.GetView<TextView>();
            if (view != null)
            {
                view.SetTextByStr(item.GetData().ToString());
            }
        }

        public override void OnSubmit(NavigationListView list, GameNavigationItem item)
        {
            MLog.Log("Test_List_1_Proxy.OnSubmit()");
        }
    }
}