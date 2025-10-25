using Cysharp.Threading.Tasks;

namespace GameFramework.Core 
{
    public class GameInitState : StateItemBase
    {
        private class Init2LoginTrigger : StateTriggerBase<GameLoginState>
        {
            public override bool Check(IBlackboard blackboard)
            {
                int v = blackboard.GetBlackboardIntValue(StateManager.PhaseKey);
                return v == (int)StateManager.GamePhase.Login;
            }
        }

        protected override void OnInit()
        {
            AddTrigger(new Init2LoginTrigger());
        }

        protected override void OnEnter()
        {
            SetBlackboardValue(StateManager.PhaseKey, (int)StateManager.GamePhase.Login);
        }
    }
}