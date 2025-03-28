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

        public override void OnEnable()
        {
            //int count = Random.Range(80, 100);

            count = 10;

            TestData[] data = new TestData[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = new TestData() { id = i };
            }
            listView.UpdateData(data);
        }

        public override void OnBindData(GameNavigationItem item)
        {
            
        }

        public override void OnUnbindData(GameNavigationItem item)
        {

        }

        public override void OnRefresh(GameNavigationItem item)
        {
            TextView view = item.GetView<TextView>();
            if (view != null)
            {
                if (item.TryGetData(out TestData data)) 
                {
                    view.SetTextByStr(data.ToString());                
                }
            }

            ImageView imageView = item.GetView<ImageView>();
            if (imageView != null) 
            {
                if (item.TryGetData(out TestData data))
                {
                    imageView.SetSprite("icon_001_" + data.id);
                }
            }
        }

        public override void OnSubmit(GameNavigationItem item)
        {
            //int count = Random.Range(80, 100);

            count = count - 3;
            count = Mathf.Max(count, 0);
            TestData[] data = new TestData[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = new TestData() { id = i };
            }
            listView.UpdateData(data);
        }
    }
}