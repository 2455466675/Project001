using Config;
using Navigation;
using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    public abstract class BattleCampComponent : UnitComponent
    {
        public virtual void Reuse(int cfgId) 
        {
        }       
    }

    public class BattleHeroComponent : BattleCampComponent
    {
        public override void Reuse(int cfgId)
        {
            HeroCfg cfg = Game.Config.Find<HeroCfg>(cfgId);
            GetComponent<ActorComponent>().Init(cfg.ActorId);
        }
    }

    public class BattleMonsterComponent : BattleCampComponent 
    {
        public override void Reuse(int cfgId)
        {
            MonsterCfg cfg = Game.Config.Find<MonsterCfg>(cfgId);
            GetComponent<ActorComponent>().Init(cfg.ActorId);
        }
    }

    public enum BattleUnitState 
    {
        Empty = 0,
        Alive = 1,
        Dead  = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public class BattleUnit : UnitArchetype<BattleActorComponent>, INavigationItemData
    {
        public int BattleId { get; private set; }

        public BattleUnitState State { get; private set; }

        public void Init(int id) 
        {
            BattleId = id;
            State = BattleUnitState.Empty;
        }

        public void Reuse(int cfgId) 
        {
            if (cfgId == 0) 
            {
                State = BattleUnitState.Empty;
            }
            else
            {
                GetComponent<BattleCampComponent>().Reuse(cfgId);
                State = BattleUnitState.Alive;
            }
        }

        public void Bind(IRefreshable obj)
        {

        }

        public void Unbind(IRefreshable obj)
        {

        }
    }
}
