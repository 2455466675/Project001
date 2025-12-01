using System;
using System.Collections.Generic;

namespace GameFramework.Core 
{
    public interface IBlackboard 
    {
        void SetBlackboardValue(string key, int value);
        void SetBlackboardValue(string key, float value);
        void SetBlackboardValue(string key, bool value);
        void SetBlackboardValue(string key, string value);
        int GetBlackboardIntValue(string key);
        float GetBlackboardFloatValue(string key);
        bool GetBlackboardBoolValue(string key);
        string GetBlackboardStringValue(string key);
    }

    public interface IStateTrigger 
    {
        Type Type { get; }
        bool Check(IBlackboard blackboard);
    }

    public interface IStateItem 
    {
        void Init(GameStateMachine machine);
        void Enter();
        void Exit();
        void Tick();
    }

    public abstract class StateTriggerBase<TriggerState> : IStateTrigger where TriggerState : IStateItem
    {
        public Type Type => typeof(TriggerState);

        public abstract bool Check(IBlackboard blackboard);
    }

    public abstract class StateItemBase : IStateItem , IBlackboard
    {
        private GameStateMachine machine;
        private List<IStateTrigger> triggers;

        public StateItemBase() 
        {
            triggers = new List<IStateTrigger>();
        }

        public void Init(GameStateMachine machine) 
        {
            this.machine = machine;
            OnInit();
        }
        public void Enter()
        {
            OnEnter();
        }

        public void Exit()
        {
            OnExit();
        }

        public void Tick()
        {
            if (!CheckTrigger())
            {
                OnTick();
            }
        }

        public void AddTrigger(IStateTrigger trigger)
        {
            triggers.Add(trigger);
        }

        protected virtual void OnInit() 
        {

        }

        protected virtual void OnEnter() 
        {
        }

        protected virtual void OnExit() 
        {              
        }

        protected virtual void OnTick()
        {
        }

        #region Blackboard
        public void SetBlackboardValue(string key, int value)
        {
            machine.SetBlackboardValue(key, value);
        }

        public void SetBlackboardValue(string key, bool value)
        {
            machine.SetBlackboardValue(key, value);
        }

        public void SetBlackboardValue(string key, string value)
        {
            machine.SetBlackboardValue(key, value);
        }

        public void SetBlackboardValue(string key, float value)
        {
            machine.SetBlackboardValue(key, value);
        }

        public int GetBlackboardIntValue(string key)
        {
            return machine.GetBlackboardIntValue(key);
        }

        public bool GetBlackboardBoolValue(string key)
        {
            return machine.GetBlackboardBoolValue(key);
        }

        public string GetBlackboardStringValue(string key)
        {
            return machine.GetBlackboardStringValue(key);
        }

        public float GetBlackboardFloatValue(string key)
        {
            return machine.GetBlackboardFloatValue(key);
        }

        #endregion

        private bool CheckTrigger() 
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                IStateTrigger trigger = triggers[i];
                bool result = trigger.Check(this);
                if (result)
                {
                    machine.Switch(trigger.Type);
                    return true;
                }
            }

            return false;
        }
    }

    public class GameStateMachine : IBlackboard
    {
        private Dictionary<string, IStateItem> states;
        private IStateItem current;
        private DataModel blackboard;

        public GameStateMachine() 
        {
            states = new Dictionary<string, IStateItem>();
            blackboard = new DataModel();
        }

        public void AddState<T>() where T : IStateItem
        {
            Type type = typeof(T);
            IStateItem state = Activator.CreateInstance(type) as IStateItem;
            AddState(state);
        }

        public void AddState(IStateItem state) 
        {
            Type type = state.GetType();
            string key = type.FullName;
            states[key] = state;
            state.Init(this);
        }

        public void Run<T>() where T : IStateItem
        {
            Type type = typeof(T);
            Run(type);
        }

        public void Run(Type type) 
        {
            string key = type.FullName;
            IStateItem state = GetState(key);
            if (state == null) 
            {
                return;
            }
            current = state;
            current.Enter();
        }

        public void Tick() 
        {
            current?.Tick();
        }

        public void Switch(Type type) 
        {
            string key = type.FullName;
            IStateItem state = GetState(key);
            if (state == null) 
            {
                return;
            }
            current?.Exit();
            current = state;
            current?.Enter();
        }

        #region Blackboard

        public void SetBlackboardValue(string key, int value) 
        {
            blackboard.SetValue(key, value);
        }

        public void SetBlackboardValue(string key, bool value)
        {
            blackboard.SetValue(key, value);
        }

        public void SetBlackboardValue(string key, string value)
        {
            blackboard.SetValue(key, value);
        }

        public void SetBlackboardValue(string key, float value)
        {
            blackboard.SetValue(key, value);
        }

        public int GetBlackboardIntValue(string key) 
        {
            return blackboard.GetIntValue(key);
        }

        public bool GetBlackboardBoolValue(string key)
        {
            return blackboard.GetBoolValue(key);
        }

        public string GetBlackboardStringValue(string key)
        {
            return blackboard.GetStringValue(key);
        }

        public float GetBlackboardFloatValue(string key)
        {
            return blackboard.GetFloatValue(key);
        }

        #endregion

        private IStateItem GetState(string key) 
        {
            states.TryGetValue(key, out IStateItem state);
            return state;
        }
    }
}
