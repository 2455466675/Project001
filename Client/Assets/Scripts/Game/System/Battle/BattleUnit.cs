
namespace Game.System
{
    public enum BattleUnitState 
    {
        Empty = 0,
        Alive = 1,
        Dead  = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public class BattleUnit : UnitArchetype<BattleActorComponent, BattleNodeComponent>
    {
        public int BattleId { get; private set; }

        public BattleUnitState State { get; private set; }

        public void Init(int id) 
        {
            BattleId = id;
            State = BattleUnitState.Empty;
        }

        public void Reset(int cfgId) 
        {
            if (cfgId == 0) 
            {
                State = BattleUnitState.Empty;
            }
            else
            {
                GetComponent<BattleCampComponent>().Reset(cfgId);
                State = BattleUnitState.Alive;
            }
        }

        //绑定战斗点位
        public void Attach(BattleNavigationItem node)
        {
            GetComponent<BattleNodeComponent>().Attach(node);

            if (State == BattleUnitState.Alive)
            {
                BattleActorComponent bac = GetComponent<BattleActorComponent>();
                bac.RefreshActor();
            }
        }

        public void Clear() 
        {
            State = BattleUnitState.Empty;

            GetComponent<BattleNodeComponent>().Clear();
            GetComponent<BattleActorComponent>().Clear();
        }
    }
}
