using Config;

namespace Game.GSystem
{
    public abstract class BattleCampComponent : UnitComponent
    {
        public int CfgId { get; protected set; }

        public virtual string Name { get; }

        public abstract void Reset(int cfgId);
    }

    public class BattleHeroComponent : BattleCampComponent
    {
        private HeroCfg cfg;
        public override string Name => cfg.Name;
        public override void Reset(int cfgId)
        {
            HeroCfg cfg = Game.Config.Find<HeroCfg>(cfgId);
            GetComponent<ActorComponent>().Init(cfg.ActorId);
            CfgId = cfgId;
            this.cfg = cfg;
        }
    }

    public class BattleMonsterComponent : BattleCampComponent
    {
        private MonsterCfg cfg;
        public override string Name => cfg.Name;
        public override void Reset(int cfgId)
        {
            MonsterCfg cfg = Game.Config.Find<MonsterCfg>(cfgId);
            GetComponent<ActorComponent>().Init(cfg.ActorId);
            CfgId = cfgId;
            this.cfg = cfg;
        }
    }
}
