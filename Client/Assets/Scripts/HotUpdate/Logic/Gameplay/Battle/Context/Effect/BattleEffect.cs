
namespace GameFramework.Logic
{
    public enum EffectOutcome
    {
        None,
        TakeDamage,
        RecoverHp,
        RecoverSp,
        AddBuff,
        RemoveBuff,
    }

    public enum HitType
    {
        None,
        Miss,
        Immunity,
        Critical,
        Absorbed,
    }

    public enum DamageType
    {
        None = 0,
        Physical = 1 << 0, 
        True = 1 << 1,
        Poison = 1 << 2,
        Metal = 1 << 3,
        Wood = 1 << 4,
        Water = 1 << 5,
        Fire = 1 << 6,
        Earth = 1 << 7,
    }

    public class EffectResult
    {
        public int SourceId;
        public int TargetId;
        public int AbilityId;
        public int Value1;
        public int Value2;
        public int Value3;
        public HitType HitType;
        public EffectOutcome Outcome;
        public DamageType DamageType;
    }

    public abstract class BattleEffect
    {
        public int SourceId { get; private set; }
        public int TargetId { get; private set; }

        public void Reset(int sourceId, int targetId)
        {
            SourceId = sourceId;
            TargetId = targetId;
        }

        public abstract EffectResult Apply(IBattleContext context);
    }

    public class EmptyEffect : BattleEffect
    {
        public override EffectResult Apply(IBattleContext context)
        {
            return null;
        }
    }

    public class NormalDamageEffect : BattleEffect
    {
        public override EffectResult Apply(IBattleContext context)
        {
            var agrs = new BattleEventArgs();
            agrs.SourceID = SourceId;
            agrs.TargetID = TargetId;
            agrs.Value = 10;
            agrs.Cancel = false;

            var result = new EffectResult();
  
            context.EventHub.Fire(BattleEventType.TakeDamageAfter, context, agrs);

            result.SourceId = SourceId;
            result.TargetId = TargetId;
            result.HitType = HitType.None;
            result.Outcome = EffectOutcome.TakeDamage;
            result.Value1 = 10;
            return result;
        }
    }
}
