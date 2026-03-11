using Codice.Client.BaseCommands;

namespace FSM
{
    public interface IState
    {
        internal void Init(IStateMachine machine);
        internal void Enter();
        internal void Exit();
        internal void Tick(float deltaTime);

        public void AddTrigger<TTarget>(IStateTrigger trigger) where TTarget : IState;
        public void AddTrigger<TTrigger, TTarget>() where TTrigger : IStateTrigger, new() where TTarget : IState;
    }
}