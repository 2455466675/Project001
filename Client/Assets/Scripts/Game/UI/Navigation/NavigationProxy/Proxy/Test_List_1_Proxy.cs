using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
        static int count;

        public override void OnEnable(NavigationListView list)
        {
            //int count = Random.Range(80, 100);

            count = 14;

            TestData[] data = new TestData[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = new TestData() { id = i };
            }
            list.UpdateData(data);
        }

        public override void OnBindData(NavigationListView list, GameNavigationItem item)
        {
            
        }

        public override void OnUnbindData(NavigationListView list, GameNavigationItem item)
        {

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
            //int count = Random.Range(80, 100);

            count = count - 3;
            TestData[] data = new TestData[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = new TestData() { id = i };
            }
            list.UpdateData(data);
        }
    }
}