
namespace GameFramework.Core 
{
     /*
         * 
         * Init => Login <=> Play
         *              
         * LoadView
         * */

    [GameModule]
    public class StateManager : IGameModule_SyncInit, IUpdate, IBlackboard
    {
        public const string PhaseKey = "GamePhase";

        public enum GamePhase
        {
            Init = 1,
            Login = 2,
            Play = 3,
        }

        private GameStateMachine machine;

        public void Init()
        {
            machine = new GameStateMachine();
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

        public bool GetBlackboardBoolValue(string key)
        {
            return machine.GetBlackboardBoolValue(key);
        }

        public int GetBlackboardIntValue(string key)
        {
            return machine.GetBlackboardIntValue(key);
        }

        public string GetBlackboardStringValue(string key)
        {
            return machine.GetBlackboardStringValue(key);
        }

        public float GetBlackboardFloatValue(string key)
        {
            return machine.GetBlackboardFloatValue(key);
        }

        public void SetBlackboardValue(string key, int value)
        {
            machine.SetBlackboardValue(key, value);
        }

        public void SetBlackboardValue(string key, bool value)
        {
            machine.SetBlackboardValue(key, value);
        }

        public void SetBlackboardValue(string key, string value)
        {
            machine.SetBlackboardValue(key, value);
        }

        public void SetBlackboardValue(string key, float value)
        {
            machine.SetBlackboardValue(key, value);
        }
    }
}
