using Config;
using GameFramework.Core;
using MVC;
using UnityEngine;

namespace GameFramework.View.UI
{
    public class LoginListItem : ObservableModel
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
        protected override void OnShow()
        {
            ObservableList<LoginListItem> items = new ObservableList<LoginListItem>();
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
                case 4:
                    Application.Quit();
                    break;                
            }
        }
    }
}