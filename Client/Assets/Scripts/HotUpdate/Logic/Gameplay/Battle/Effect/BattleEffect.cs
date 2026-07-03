
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
            MDebug.Log("NormalDamageEffect Apply");
            context.Projector.Record(new TestViewCommand());
        }
    }
}
