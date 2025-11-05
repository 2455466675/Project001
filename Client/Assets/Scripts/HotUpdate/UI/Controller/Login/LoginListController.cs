using Config;
using GameFramework.Core;
using System.Collections.Generic;

namespace GameFramework.UI
{
    [NavigationController(NavigationDefine.LoginList)]
    public class LoginListController : NavigationController
    {
        protected override bool CheckIsLocked()
        {
            int v = Game.GetModule<StateManager>().GetBlackboardIntValue(StateManager.PhaseKey);
            return v != (int)StateManager.GamePhase.Play;
        }

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
            int id = dataModel.GetIntValue("id");

            TextWidget text = itemView.GetWidget<TextWidget>();
            if (text != null) 
            {              
                text.SetTextById(dataModel.GetStringValue("name"));
            }

            ImageWidget image = itemView.GetWidget<ImageWidget>();
            if (image != null) 
            {
                if (id == 2) 
                {
                    bool hasAnySaveData = Game.GetModule<SaveManager>().HasAnySaveData();
                    image.SetActive(!hasAnySaveData);
                }
                else
                {
                    image.SetActive(false);
                }
            }
        }

        protected override void SubmitItemView(NavigationItemView itemView, DataModel dataModel)
        {
            int id = dataModel.GetIntValue("id");
            MDebug.Log("SubmitItemView", id);
            if (id == 1) 
            {
                Game.GetModule<StateManager>().SetBlackboardValue(StateManager.PhaseKey, (int)StateManager.GamePhase.Play);
                Game.GetModule<StateManager>().SetBlackboardValue("SaveIndex", 0);
                Game.GetModule<UIManager>().ExitNavigate();
            }
        }

        protected override bool CheckItemIsValid(DataModel dataModel)
        {
            int id = dataModel.GetIntValue("id");
            if (id != 2) 
            {
                return true;
            }
            return Game.GetModule<SaveManager>().HasAnySaveData();
        }
    }
}
