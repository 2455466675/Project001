using UnityEngine;

namespace Game.UI
{
    public class TestData 
    {
        public int id;

        public override string ToString()
        {
            return id.ToString();
        }
    }

    [ListProxy(NavigationListDefine.Test_List_1)]
    public class Test_List_1_Proxy : NavigationListProxy
    {
        public override void OnEnable(NavigationListView list)
        {
            int count = Random.Range(5, 20);

            TestData[] data = new TestData[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = new TestData() { id = i };
            }
            list.UpdateData(data);
        }

        public override void OnBindData(NavigationListView list, GameNavigationItem item)
        {
            MLog.Log("OnBindData", item.GetData().ToString());
        }

        public override void OnUnbindData(NavigationListView list, GameNavigationItem item)
        {
            MLog.Log("OnUnbindData", item.GetData().ToString());
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