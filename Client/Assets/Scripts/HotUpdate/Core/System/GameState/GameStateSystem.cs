using FSM;

namespace GameFramework.Core
{
    public enum GamePhase
    {
        Init = 1,
        Login = 2,
        Play = 3,
    }

    public class LoginTrigger : IStateTrigger
    {
        public bool Check(IBlackboard blackboard)
        {
            return Game.GetSystem<GameStateSystem>().GamePhase == GamePhase.Login;
        }
    }

    public class PlayTrigger : IStateTrigger
    {
        public bool Check(IBlackboard blackboard)
        {
            return Game.GetSystem<GameStateSystem>().GamePhase == GamePhase.Play;
        }
    }

    [GameSystem]
    public class GameStateSystem : IGameSystem, IInit, IUpdateable
    {
        private StateMachine machine;        
        
        public GamePhase GamePhase { get; set; }

        public void Init()
        {
            machine = new StateMachine();
            machine.AddState(new GameInitState());
            machine.AddState(new GameLoginState());
            machine.AddState(new GamePlayState());
        }

        public void Update(float deltaTime)
        {
            machine.Update(deltaTime);
        }

        public void Start()
        {
            machine.Run<GameInitState>();
        }
    }
}