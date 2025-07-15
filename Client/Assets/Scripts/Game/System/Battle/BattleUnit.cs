using Cysharp.Threading.Tasks;

namespace Game.GSystem
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
    public class BattleUnit : UnitArchetype<BattleActorComponent, BattleNodeComponent, BattleTransformComponent, BattleAttributeComponent>
    {
        public int BattleId { get; private set; }

        public BattleUnitState State { get; private set; }

        public void Init(int battleId) 
        {
            BattleId = battleId;
            State = BattleUnitState.Empty;
            AddComponent<BattleHudComponent>();
            AddComponent<BattleBehaviorComponent>();
            AddComponent<BattleAIComponent>();
        }

        public void Reset(int cfgId) 
        {
            if (cfgId == 0) 
            {
                State = BattleUnitState.Empty;
            }
            else
            {
                State = BattleUnitState.Alive;
                GetComponent<BattleCampComponent>().Reset(cfgId);
                GetComponent<BattleAttributeComponent>().SetAttributeValue(AttributeDefine.HP_1, 100);
                GetComponent<BattleAttributeComponent>().SetAttributeValue(AttributeDefine.HP_2, 80);
                GetComponent<BattleAttributeComponent>().SetAttributeValue(AttributeDefine.SP_1, 100);
                GetComponent<BattleAttributeComponent>().SetAttributeValue(AttributeDefine.SP_2, 0);
                GetComponent<BattleAttributeComponent>().SetAttributeValue(AttributeDefine.SP_RATE, 1);
                GetComponent<BattleAttributeComponent>().SetAttributeValue(AttributeDefine.ATK, 10);
            }
        }

        public void Clear() 
        {
            State = BattleUnitState.Empty;
            GetComponent<BattleNodeComponent>().Clear();
            GetComponent<BattleActorComponent>().Clear();
        }

        public void Attach(BattleNavigationItem node)
        {
            GetComponent<BattleNodeComponent>().Attach(node);
        }

        public async UniTask RefreshActorAsync() 
        {
            if (State == BattleUnitState.Alive)
            {
                BattleActorComponent bac = GetComponent<BattleActorComponent>();
                await bac.RefreshActorAsync();
            }
        }

        public ActionHandle PlayAction(int hash, object userData = null) 
        {
            return GetComponent<ActorComponent>().PlayAction(hash, userData);
        }
    }
}
