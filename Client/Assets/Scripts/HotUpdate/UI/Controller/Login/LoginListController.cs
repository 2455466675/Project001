using Config;
using GameFramework.Core;
using System.Collections.Generic;

namespace GameFramework.UI
{
    [NavigationController(NavigationDefine.LoginList)]
    public class LoginListController : NavigationController
    {
        protected override void OnShow()
        {
            LoginCfg[] cfgs = Game.GetModule<ConfigManager>().FindAll<LoginCfg>();
            List<DataModel> list = new List<DataModel>();
            for (int i = 0; i < cfgs.Length; i++)
            {
                LoginCfg cfg = cfgs[i];
                DataModel data = new DataModel();
                data.SetValue("id", cfg.Id);
                data.SetValue("name", cfg.Name);
                list.Add(data);
            }

            SetData(list);
        }

        protected override void RefreshItemView(NavigationItemView itemView, DataModel dataModel)
        {
            TextWidget text = itemView.GetWidget<TextWidget>();
            if (text != null) 
            {              
                text.SetText(dataModel.GetIntValue("name"));
            }
        }
    }
}
