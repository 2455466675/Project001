using GameFramework.Logic;
using MVC;

namespace GameFramework.View.UI
{
    [NavigationController(Utility.GameDefine.NavigationDefine.BattlePlayer)]
    public class BattleFormationPlayerController : NavigationController<BattleFormationSite>
    {
        protected override void RegisterData()
        {
            MDebug.Log("战斗阵型-玩家");

            DataModelList<BattleFormationSite> list = new DataModelList<BattleFormationSite>();

            SetData(list);
        }

        protected override void BindItemView(NavigationItemView itemView, BattleFormationSite dataModel, Binder binder)
        {           
            itemView.transform.position = dataModel.Position;
        }

        protected override void SelectItemView(NavigationItemView itemView, BattleFormationSite dataModel)
        {
            MDebug.Log($"战斗阵型-玩家:SelectItemView{dataModel.Index}");
        }

        protected override bool CheckItemIsValid(BattleFormationSite dataModel)
        {
            return dataModel.Valid && dataModel.State == 1;
        }
    }
}
