using Cysharp.Threading.Tasks;
using FSM;

namespace GameFramework.Core
{
    public class GameLoginState : StateBase
    {
        protected override void OnInit()
        {
            AddTrigger<PlayTrigger, GamePlayState>();
        }

        protected override void OnEnter()
        {
            Load().Forget();
        }

        protected override void OnExit()
        {
            Game.Message.SendMessage(new GameLoginMessage() { status = 2 });
        }

        private async UniTaskVoid Load()
        {
            Game.Message.SendMessage(new GameLoginMessage() { status = 0 });
            await Game.GetSystem<GameSceneSystem>().LoadScene(1002);
            Game.Message.SendMessage(new GameLoginMessage() { status = 1 });
        }
    }
}