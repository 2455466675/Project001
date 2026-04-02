using System;
using System.Collections.Generic;

namespace FSM
{
    public abstract class StateBase : IState, IBlackboard
    {
        private class TriggerHandler : IStateTrigger
        {
            public Type TargetType { get; private set; }
            private IStateTrigger trigger;

            public TriggerHandler(Type targetType, IStateTrigger trigger)
            {
                this.TargetType = targetType;
                this.trigger = trigger;
            }

            public bool Check(IBlackboard blackboard)
            {
                return trigger.Check(blackboard);
            }
        }

        private List<TriggerHandler> triggers = new List<TriggerHandler>();
        private IStateMachine machine;

        public void AddTrigger<TTarget>(IStateTrigger trigger) where TTarget : IState
        {
            triggers.Add(new TriggerHandler(typeof(TTarget), trigger));
        }

        public void AddTrigger<TTrigger, TTarget>() where TTrigger : IStateTrigger, new() where TTarget : IState
        {
            triggers.Add(new TriggerHandler(typeof(TTarget), new TTrigger()));
        }

        #region IState

        void IState.Init(IStateMachine machine)
        {
            this.machine = machine;
            OnInit();
        }

        void IState.Enter()
        {
            OnEnter();
        }

        void IState.Exit()
        {
            OnExit();
        }

        void IState.Tick(float deltaTime)
        {
            if (!CheckTrigger())
            {
                OnTick();
            }
        }

        #endregion

        #region IBlackboard

        public void SetValue(string key, int value)
        {
            machine.SetValue(key, value);
        }

        public void SetValue(string key, float value)
        {
            machine.SetValue(key, value);
        }

        public void SetValue(string key, bool value)
        {
            machine.SetValue(key, value);
        }

        public void SetValue(string key, string value)
        {
            machine.SetValue(key, value);
        }

        public int GetIntValue(string key)
        {
            return machine.GetIntValue(key);
        }

        public float GetFloatValue(string key)
        {
            return machine.GetFloatValue(key);
        }

        public bool GetBoolValue(string key)
        {
            return machine.GetBoolValue(key);
        }

        public string GetStringValue(string key)
        {
            return machine.GetStringValue(key);
        }

        #endregion

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

        private bool CheckTrigger()
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                TriggerHandler handler = triggers[i];
                bool result = handler.Check(machine);
                if (result)
                {
                    machine.Switch(handler.TargetType);
                    return true;
                }
            }
            return false;
        }
    }
}