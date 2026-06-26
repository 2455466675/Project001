using GameFramework.Logic;

namespace GameFramework.View.UI
{
    [NavigationController(Utility.GameDefine.NavigationDefine.BattlePlayer)]
    public class BattleFormationPlayerController : NavigationController<BattleUnitData>
    {
        protected override void RegisterData()
        {
            MDebug.Log("战斗阵型-玩家");

            DataModelList<BattleUnitData> list = new DataModelList<BattleUnitData>();

            for (int i = 0; i < 4; i++)
            {
                BattleUnitData model = new BattleUnitData();
                model.Id = i;
                list.Add(model);
            }

            SetData(list);
        }

        protected override void SelectItemView(NavigationItemView itemView, BattleUnitData dataModel)
        {
            MDebug.Log($"战斗阵型-玩家:SelectItemView{dataModel.Id}");
        }
    }
}
