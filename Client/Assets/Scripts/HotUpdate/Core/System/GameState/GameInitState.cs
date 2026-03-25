using FSM;

namespace GameFramework.Core
{
    public class GameInitState : StateBase
    {
        protected override void OnInit()
        {
            AddTrigger<LoginTrigger, GameLoginState>();
        }

        protected override void OnEnter()
        {
            Game.GetSystem<GameStateSystem>().GamePhase = GamePhase.Login;
        }
    }
}