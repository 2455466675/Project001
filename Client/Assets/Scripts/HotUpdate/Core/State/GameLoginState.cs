using Cysharp.Threading.Tasks;

namespace GameFramework.Core
{
    public struct GameLoginEventArgs : IGameEventArgs
    {
        public bool isEnter;
    }

    public class GameLoginState : StateItemBase
    {
        private class Login2PlayTrigger : StateTriggerBase<GamePlayState>
        {
            public override bool Check(IBlackboard blackboard)
            {
                int v = blackboard.GetBlackboardIntValue(StateManager.PhaseKey);
                return v == (int)StateManager.GamePhase.Play;
            }
        }

        protected override void OnInit()
        {
            AddTrigger(new Login2PlayTrigger());
        }

        protected override void OnEnter()
        {
            LoadLogin().Forget();
        }

        protected override void OnExit()
        {
            Game.Event.Publish(new GameLoginEventArgs { isEnter = false });
        }

        private async UniTaskVoid LoadLogin()
        {
            Game.Event.Publish(new GameLoginEventArgs { isEnter = true });

            await Game.GetModule<SceneManager>().LoadScene(2001);

            Game.Event.Publish(new SwitchInputModuleEventArgs() { moduleType = InputModuleType.Character });
            Game.Event.Publish(new NavigateEventArgs() { navigationDefine = NavigationDefine.LoginList });
        }
    }
}