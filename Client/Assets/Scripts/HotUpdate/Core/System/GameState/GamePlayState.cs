using Cysharp.Threading.Tasks;
using FSM;

namespace GameFramework.Core
{
    public enum GamePlayStatus
    {
        Begin = 0,
        Loaded = 1,
        Exit = 2,
    }

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
            Game.Message.SendMessage(new GamePlayMessage() { status = GamePlayStatus.Exit });
            Game.GameplayExit();
        }

        private async UniTaskVoid Load()
        {

            Game.Message.SendMessage(new GamePlayMessage() { status = GamePlayStatus.Begin });
            Game.GetSystem<GameSaveSystem>().LoadGame(GetIntValue("save_index"));
            await Game.GetSystem<GameTransitionManager>().Transition(Utility.GameDefine.TransitionType.LoadGame, LoadGame);
            Game.Message.SendMessage(new GamePlayMessage() { status = GamePlayStatus.Loaded });
        }

        private async UniTask LoadGame()
        {
            await Game.GetSystem<GameSceneSystem>().LoadScene(1001);
            Game.GetSystem<GameInputSystem>().Switch(InputModuleType.Normal);
        }
    }
}