using Cysharp.Threading.Tasks;

namespace GameFramework.Core 
{
    public class GameInitState : StateItemBase
    {
        private const string key = "LoadLoginScene_Finish";

        private class Init2LoginTrigger : StateTriggerBase<GameLoginState>
        {
            public override bool Check(IBlackboard blackboard)
            {
                bool r = blackboard.GetBlackboardBoolValue(key);
                return r;
            }
        }

        protected override void OnInit()
        {
            AddTrigger(new Init2LoginTrigger());
        }

        protected override void OnEnter()
        {
            LoadLoginScene().Forget();
        }

        private async UniTaskVoid LoadLoginScene() 
        {
            SetBlackboardValue(key, false);
            await Game.GetModule<SceneManager>().LoadScene(2001);
            SetBlackboardValue(key, true);
        }
    }
}