
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
        Critical,
        Immune,
    }

    public class EffectResult
    {
        public int SourceID;
        public int TargetID;
        public int AbilityID;
        public int Value1;
        public int Value2;
        public int Value3;
        public HitType HitType;
        public EffectOutcome Outcome;
    }

    public abstract class BattleEffect
    {
        public int SourceID { get; private set; }
        public int TargetID { get; private set; }
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
            agrs.SourceID = SourceID;
            agrs.TargetID = TargetID;
            agrs.Value = 10;
            agrs.Cancel = false;

            context.EventHub.Fire(BattleEventType.TakeDamageBefor, context, agrs);

            var result = new EffectResult();
            if (agrs.Cancel)
            {
                result.SourceID = SourceID;
                result.TargetID = TargetID;
                result.HitType = HitType.Immune;
                result.Outcome = EffectOutcome.TakeDamage;
                return result;
            }
            context.EventHub.Fire(BattleEventType.TakeDamageAfter, context, agrs);

            result.SourceID = SourceID;
            result.TargetID = TargetID;
            result.HitType = HitType.None;
            result.Outcome = EffectOutcome.TakeDamage;
            result.Value1 = 10;
            return result;
        }
    }
}
