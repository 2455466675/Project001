using Game.Cfg;
using MVC;
using Navigation;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class BattleEnemyRole : BattleRole
    {
        public override string ActorPath => cfg != null ? cfg.PrefabPath : string.Empty;
        public override BattleRoleType FightCharacterType => BattleRoleType.Enemy;

        private MonsterCfg cfg;

        public BattleEnemyRole(int index, DataContainer container) : base(index, container)
        {
        }

        public override void Reset()
        {
            StaticNavigationList list = NavigationList.GetNavigationList(UI.ListName.BattleEnemyList) as StaticNavigationList;
            FightPoint = list.GetItem(Index) as BattlePointItem;
            SetBaseValue("valid", false);
            cfg = null;
        }

        public override void UpdateCfg(int cfgId)
        {
            if (FightPoint == null)
            {
                MLog.Error($"孔位尚未初始化:{Index}");
                return;
            }

            if (cfgId <= 0)
            {
                SetBaseValue("valid", false);
                return;
            }

            cfg = GameCore.Cfg.Find<MonsterCfg>(cfgId);
            if (cfg == null)
            {
                SetBaseValue("valid", false);
                MLog.Error($"没有此配置:{cfgId}，{Index}");
                return;
            }

            RefreshActor();
        }
    }
}

