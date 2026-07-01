using GameFramework.Logic;
using MVC;

namespace GameFramework.View.UI
{
    public class BattleUnitData : DataModel
    {
        private int id;
        public int Id 
        { 
            get 
            {
                return id; 
            }
            set
            {
                SetValue(ref id, value);
            }
        }
    }

    [NavigationController(Utility.GameDefine.NavigationDefine.BattleEnemy)]
    public class BattleFormationEnemyController : NavigationController<BattleUnitData>
    {
        protected override void RegisterData()
        {
            MDebug.Log("战斗阵型-敌人");

            Logic.DataModelList<BattleUnitData> list = new Logic.DataModelList<BattleUnitData>();

            for (int i = 0; i < 6; i++)
            {
                BattleUnitData model = new BattleUnitData();
                model.Id = i;
                list.Add(model);
            }

            SetData(list);
        }

        protected override void SelectItemView(NavigationItemView itemView, BattleUnitData dataModel)
        {
            MDebug.Log($"战斗阵型-敌人:SelectItemView{dataModel.Id}");
        }
    }
}