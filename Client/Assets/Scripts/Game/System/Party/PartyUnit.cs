using Config;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class PartyUnit : RoleUnit
    {
        private ActorComponent actorComponent;
        private MotorComponent motorComponent;
        
        protected override void OnConstructor()
        {
            AddComponent<PartyComponent>();
            actorComponent = AddComponent<ActorComponent>();
            motorComponent = AddComponent<MotorComponent>();
        }

        public void Init(int id)
        {
            HeroCfg cfg = Game.Config.Find<HeroCfg>(id);
            actorComponent.Init(cfg.ActorId);
            actorComponent.Refresh();
        }

        public void Move(float x, float y)
        {
            motorComponent.Move(x, y);
        }

        public void Run(bool isRunning)
        {
            motorComponent.Run(isRunning);
        }
    }
}
