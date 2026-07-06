
namespace GameFramework.Logic
{
    public abstract class BattleEffect
    {
        public int SourceID { get; private set; }
        public int TargetID { get; private set; }
        public abstract void Apply(IBattleContext context);
    }

    public class EmptyEffect : BattleEffect
    {
        public override void Apply(IBattleContext context)
        {
        }
    }

    public class NormalDamageEffect : BattleEffect
    {
        public override void Apply(IBattleContext context)
        {
            var agrs = new BattleEventArgs();
            agrs.SourceID = SourceID;
            agrs.TargetID = TargetID;
            agrs.Value = 10;
            agrs.Cancel = false;

            context.EventHub.Fire(BattleEventType.TakeDamageBefor, context, agrs);

            if (agrs.Cancel)
            {
                MDebug.Log("NormalDamageEffect效果取消");
                return;
            }

            // Target hp-=10

            MDebug.Log("NormalDamageEffect Apply");
            context.Projector.Record(new TestViewCommand());

            context.EventHub.Fire(BattleEventType.TakeDamageAfter, context, agrs);
        }
    }
}
