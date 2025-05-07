using Config;

namespace Game.System
{
    public abstract class BattleCampComponent : UnitComponent
    {
        public abstract void Reset(int cfgId);
    }

    public class BattleHeroComponent : BattleCampComponent
    {
        public override void Reset(int cfgId)
        {
            HeroCfg cfg = Game.Config.Find<HeroCfg>(cfgId);
            GetComponent<ActorComponent>().Init(cfg.ActorId);
        }
    }

    public class BattleMonsterComponent : BattleCampComponent
    {
        public override void Reset(int cfgId)
        {
            MonsterCfg cfg = Game.Config.Find<MonsterCfg>(cfgId);
            GetComponent<ActorComponent>().Init(cfg.ActorId);
        }
    }
}
