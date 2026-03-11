using System;

namespace FSM
{
    public interface IStateMachine : IBlackboard
    {
        public void AddState<T>() where T : IState, new();
        public void AddState(IState state);
        public void Run<T>() where T : IState;
        public void Update(float deltaTime);
        internal void Switch(Type type);
    }
}