using ECS;
using FSM;
using UnityEngine;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

namespace GameFramework.Logic
{
    public class ComponentState : StateBase
    {
        private ComponentBase component;
        public ComponentState(ComponentBase component)
        {
            this.component = component;
        }

        protected T GetComponent<T>() where T : ComponentBase
        {
            return component.GetComponent<T>();
        }
    }

    public class MotorAnimatorComponent : ComponentBase, IFixedUpdateableComponent
    {
        private class Tirgger1 : IStateTrigger
        {
            public bool Check(IBlackboard blackboard)
            {
                float x = blackboard.GetFloatValue("VelocityX");
                float y = blackboard.GetFloatValue("VelocityY");
                return x != 0 || y != 0;
            }
        }

        private class Tirgger2 : IStateTrigger
        {
            public bool Check(IBlackboard blackboard)
            {
                float x = blackboard.GetFloatValue("VelocityX");
                float y = blackboard.GetFloatValue("VelocityY");
                return x == 0f && y == 0f;
            }
        }

        private class Tirgger3 : IStateTrigger
        {
            public bool Check(IBlackboard blackboard)
            {
                float x = blackboard.GetFloatValue("VelocityX");
                float y = blackboard.GetFloatValue("VelocityY");
                bool leftShift = blackboard.GetBoolValue("LeftShift");
                return (x != 0f || y != 0f) && leftShift;
            }
        }

        private class Tirgger4 : IStateTrigger
        {
            public bool Check(IBlackboard blackboard)
            {
                float x = blackboard.GetFloatValue("VelocityX");
                float y = blackboard.GetFloatValue("VelocityY");
                bool leftShift = blackboard.GetBoolValue("LeftShift");
                return (x == 0f && y == 0f) || !leftShift;
            }
        }

        private class IdleState : ComponentState
        {
            public IdleState(MotorAnimatorComponent component) : base(component)
            {
            }

            protected override void OnInit()
            {
                AddTrigger<Tirgger1, WalkState>();
            }
        }

        private class WalkState : ComponentState
        {
            public WalkState(MotorAnimatorComponent component) : base(component)
            {
            }

            protected override void OnInit()
            {
                AddTrigger<Tirgger2, IdleState>();
                AddTrigger<Tirgger3, RunState>();
            }

            protected override void OnEnter()
            {
                var ac = GetComponent<ActorComponent>();
                ac.SetAnimatorValue("IsWalk", true);
            }

            protected override void OnExit()
            {
                float x = GetFloatValue("VelocityX");
                float y = GetFloatValue("VelocityY");
                bool leftShift = GetBoolValue("LeftShift");
                var ac = GetComponent<ActorComponent>();
                ac.SetAnimatorValue("IsWalk", (x != 0 || y != 0) && leftShift);
            }
        }

        private class RunState : ComponentState
        {
            public RunState(MotorAnimatorComponent component) : base(component)
            {
            }

            protected override void OnInit()
            {
                AddTrigger<Tirgger4, WalkState>();
            }

            protected override void OnEnter()
            {
                var ac = GetComponent<ActorComponent>();
                ac.SetAnimatorValue("IsRun", true);
            }

            protected override void OnExit()
            {
                var ac = GetComponent<ActorComponent>();
                ac.SetAnimatorValue("IsRun", false);
            }
        }


        private bool isStartUp;
        private StateMachine stateMachine;
        private ActorComponent actorComponent;

        private float dirX;
        private float dirY;

        void IFixedUpdateableComponent.FixedUpdate(float fixedDeltaTime)
        {
            if (!isStartUp)
            {
                return;
            }

            stateMachine.Update(fixedDeltaTime);
        }

        protected override void Awake()
        {
            stateMachine = new StateMachine();
            stateMachine.AddState(new IdleState(this));
            stateMachine.AddState(new WalkState(this));
            stateMachine.AddState(new RunState(this));
        }

        protected override void Start()
        {
            actorComponent = GetComponent<ActorComponent>();
        }

        public void StartUp()
        {
            isStartUp = true;
            stateMachine.Run<IdleState>();
            RefreshDirection();
        }

        public void ShutDown()
        {
            isStartUp = false;
        }

        public void SetVelocity(float x, float y)
        {
            if ((x != 0f || y != 0f) && (x != dirX || y != dirY))
            {
                dirX = x;
                dirY = y;
                RefreshDirection();
            }

            stateMachine.SetValue("VelocityX", x);
            stateMachine.SetValue("VelocityY", y);
        }

        public void SetLeftShift(bool v)
        {
            stateMachine.SetValue("LeftShift", v);
        }

        private void RefreshDirection()
        {
            actorComponent.SetAnimatorValue("DirX", dirX);
            actorComponent.SetAnimatorValue("DirY", dirY);
        }
    }
}