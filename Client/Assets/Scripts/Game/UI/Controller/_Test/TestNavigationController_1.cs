using GameFramework.Featrue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UI 
{
    [NavigationController(NavigationDefine.TestList1)]
    public class TestNavigationController_1 : NavigationController
    {
        protected override void OnShow()
        {
            MDebug.Log("NavigationDefine.TestList1 OnShow");
            List<DataModel> datas = new List<DataModel>();

            for (int i = 0; i < 20; i++)
            {
                DataModel data = new DataModel();
                data.SetValue("id", i);
                datas.Add(data);
            }

            SetData(datas);
        }

        protected override void RefreshItemView(NavigationItemView itemView, DataModel dataModel)
        {
            TextWidget textWidget = itemView.GetWidget<TextWidget>();
            if (textWidget != null) 
            {
                int id = dataModel.GetIntValue("id");
                textWidget.SetText(string.Format("id:{0}", id));
            }
        }

        protected override void SubmitItemView(NavigationItemView itemView, DataModel dataModel)
        {
            Game.GetModule<UIManager>().Navigate(NavigationDefine.TestList2);
        }

        protected override bool CheckIsLocked()
        {
            return true;
        }
    }

    [NavigationController(NavigationDefine.TestList2)]
    public class TestNavigationController_2 : NavigationController
    {
        protected override void OnShow()
        {
            MDebug.Log("NavigationDefine.TestList2 OnShow");
            List<DataModel> datas = new List<DataModel>();

            for (int i = 0; i < 4; i++)
            {
                DataModel data = new DataModel();
                data.SetValue("id", i);
                datas.Add(data);
            }

            SetData(datas);
        }

        protected override void RefreshItemView(NavigationItemView itemView, DataModel dataModel)
        {
            TextWidget textWidget = itemView.GetWidget<TextWidget>();
            if (textWidget != null)
            {
                int id = dataModel.GetIntValue("id");
                textWidget.SetText(string.Format("id:{0}", id));
            }
        }
    }
}