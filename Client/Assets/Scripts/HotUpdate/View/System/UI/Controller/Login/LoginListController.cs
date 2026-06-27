using Config;
using GameFramework.Core;
using GameFramework.Logic;
using MVC;
using UnityEngine;

namespace GameFramework.View.UI
{
    public class LoginListItem : DataModel
    {
        private int id;
        public int Id
        {
            get { return id; } 
            set { SetValue(ref id, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetValue(ref name, value); }
        }
    }

    [NavigationController(Utility.GameDefine.NavigationDefine.LoginList)]
    public class LoginListController : NavigationController<LoginListItem>
    {
        protected override void RegisterData()
        {
            Logic.DataModelList<LoginListItem> items = new Logic.DataModelList<LoginListItem>();
            LoginCfg[] cfgs = Game.Config.FindAll<LoginCfg>();
            foreach (LoginCfg cfg in cfgs)
            {
                LoginListItem item = new LoginListItem();
                item.Id = cfg.Id;
                item.Name = cfg.Name;
                items.Add(item);
            }
            SetData(items);
        }

        protected override void BindItemView(NavigationItemView itemView, LoginListItem dataModel, Binder binder)
        {
            TextWidget textWidget = itemView.GetWidget<TextWidget>();
            if (textWidget != null)
            {
                binder.Binding(textWidget, dataModel, d => d.Name, (o, s) => 
                {
                    o.SetTextById(s.Name);
                });
            }
        }

        protected override void SubmitItemView(NavigationItemView itemView, LoginListItem dataModel)
        {
            int id = dataModel.Id;

            switch (id)
            {
                case 1:
                    Game.GetSystem<GameStateSystem>().GamePhase = GamePhase.Play;
                    break;
                case 2:
                    Game.GetSystem<UISystem>().Navigate(Utility.GameDefine.NavigationDefine.GameSaveList, null, GameSaveType.Read);
                    break;
                case 4:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    break;                
            }
        }

        protected override bool CheckItemIsValid(LoginListItem dataModel)
        {
            int id = dataModel.Id;
            if (id != 2)
            {
                return true;
            }

            return Game.GetSystem<GameSaveSummary>().HasAnySaveData();
        }
    }
}