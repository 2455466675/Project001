
namespace GameFramework.Core 
{
     /*
         * 
         * Init => Login <=> Play
         *              
         * LoadView
         * */

    [GameModule(GameModulePriority.StateManager)]
    public class StateManager : IGameModule_SyncInit, IUpdate
    {
        private StateMachine machine;

        public void Init()
        {
            machine = new StateMachine();
            machine.AddState(new GameInitState());
            machine.AddState(new GameLoginState());
            machine.AddState(new GamePlayState());
        }

        public void Start() 
        {
            machine.Run<GameInitState>();
        }

        public void Update()
        {
            machine.Tick();
        }
    }
}
