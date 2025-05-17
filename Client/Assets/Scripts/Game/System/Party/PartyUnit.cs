using Config;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class PartyUnit : UnitArchetype<PartyComponent, SceneActorComponent, MotorComponent>
    {
        public HeroCfg Config { get; private set; }
        private ActorComponent actorComponent;
        private MotorComponent motorComponent;
        
        protected override void OnInitUnit()
        {
            actorComponent = GetComponent<ActorComponent>();
            motorComponent = GetComponent<MotorComponent>();
        }

        public void Init(int id)
        {
            HeroCfg cfg = Game.Config.Find<HeroCfg>(id);
            actorComponent.Init(cfg.ActorId);
            actorComponent.RefreshActor();
            Config = cfg;
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
