using System;
using System.Collections.Generic;

namespace FSM
{
    public class StateMachine : IStateMachine
    {
        private Dictionary<string, IState> states;
        private IState current;
        private IBlackboard blackboard;

        public StateMachine()
        {
            states = new Dictionary<string, IState>();
            blackboard = new Blackboard();
        }

        public void AddState<T>() where T : IState, new()
        {
            IState state = new T();
            AddState(state);
        }

        public void AddState(IState state)
        {
            Type type = state.GetType();
            string key = type.FullName;
            states[key] = state;
            state.Init(this);
        }

        public void Run<T>() where T : IState
        {
            Type type = typeof(T);
            Run(type);
        }

        public void Update(float deltaTime)
        {
            current?.Tick(deltaTime);
        }

        private void Run(Type type)
        {
            string key = type.FullName;
            IState state = GetState(key);
            if (state == null)
            {
                return;
            }
            current = state;
            current.Enter();
        }

        private IState GetState(string key)
        {
            states.TryGetValue(key, out IState state);
            return state;
        }

        void IStateMachine.Switch(Type type)
        {
            string key = type.FullName;
            IState state = GetState(key);
            if (state == null)
            {
                return;
            }
            current?.Exit();
            current = state;
            current?.Enter();
        }

        #region IBlackboard

        public void SetValue(string key, int value)
        {
            blackboard.SetValue(key, value);
        }

        public void SetValue(string key, float value)
        {
            blackboard.SetValue(key, value);
        }

        public void SetValue(string key, bool value)
        {
            blackboard.SetValue(key, value);
        }

        public void SetValue(string key, string value)
        {
            blackboard.SetValue(key, value);
        }

        public int GetIntValue(string key)
        {
            return blackboard.GetIntValue(key);
        }

        public float GetFloatValue(string key)
        {
            return blackboard.GetFloatValue(key);
        }

        public bool GetBoolValue(string key)
        {
            return blackboard.GetBoolValue(key);
        }


        public string GetStringValue(string key)
        {
            return blackboard.GetStringValue(key);
        }

        #endregion
    }
}