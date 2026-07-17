namespace GameFramework.Logic
{
    public interface IBattleContext
    {
        IActionSchedule Schedule { get; }
        IEffectResolver Resolver { get; }
        IViewProjector Projector { get; }
        IEventHub EventHub { get; }
        IDamagePipeline DamagePipeline { get; }

        bool CheckFinish();
    }

    public class BattleContext : IBattleContext
    {
        public IActionSchedule Schedule { get; private set; }
        public IEffectResolver Resolver { get; private set; }
        public IViewProjector Projector { get; private set; }
        public IEventHub EventHub { get; private set; }
        public IDamagePipeline DamagePipeline { get; private set; }

        public BattleContext()
        {
            Schedule = new ActionSchedule();
            Resolver = new EffectResolver();
            Projector = new ViewProjector();
            EventHub = new EventHub();
            DamagePipeline = new DamagePipeline();
        }

        public bool CheckFinish()
        {
            return false;
        }
    }
}
