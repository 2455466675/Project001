using Cysharp.Threading.Tasks;
using FSM;

namespace GameFramework.Core
{
    public class GamePlayState : StateBase
    {
        protected override void OnInit()
        {
            AddTrigger<LoginTrigger, GameLoginState>();
        }

        protected override void OnEnter()
        {
            Load().Forget();
        }

        protected override void OnExit()
        {
            Game.Message.SendMessage(new GamePlayMessage() { status = 2 });
            Game.GameplayExit();
        }

        private async UniTaskVoid Load()
        {
            Game.Message.SendMessage(new GamePlayMessage() { status = 0 });
            await Game.GetSystem<GameSceneSystem>().LoadScene(1001);
            await Game.GetSystem<GameSceneSystem>().LoadBattleScene();
            Game.Message.SendMessage(new GamePlayMessage() { status = 1 });
        }
    }
}