using ECS;

namespace GameFramework.Logic
{
    public interface IBattleContext
    {
        IActionSchedule Schedule { get; }
        IViewProjector Projector { get; }
        IEventHub EventHub { get; }
        bool CheckFinish();
        Entity GetBattleUnit(int battleId);
    }

    public class BattleContext : IBattleContext
    {
        public IActionSchedule Schedule { get; private set; }
        public IViewProjector Projector { get; private set; }
        public IEventHub EventHub { get; private set; }

        public BattleContext()
        {
            Schedule = new ActionSchedule();
            Projector = new ViewProjector();
            EventHub = new EventHub();
        }

        public bool CheckFinish()
        {
            return false;
        }

        public Entity GetBattleUnit(int battleId)
        {
            return Game.GetModule<BattleModule>().Unit.GetBattleUnit(battleId);
        }
    }
}
