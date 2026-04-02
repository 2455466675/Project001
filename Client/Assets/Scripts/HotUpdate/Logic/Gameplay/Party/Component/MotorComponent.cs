using ECS;
using FSM;
using UnityEngine;
using System.Collections.Generic;
using GameFramework.Core;
using GameFramework.Utility;

namespace GameFramework.Logic
{
    public class Tirgger1 : IStateTrigger
    {
        public bool Check(IBlackboard blackboard)
        {
            float x = blackboard.GetFloatValue("VelocityX");
            float y = blackboard.GetFloatValue("VelocityY");
            return x != 0 || y != 0;
        }
    }

    public class Tirgger2 : IStateTrigger
    {
        public bool Check(IBlackboard blackboard)
        {
            float x = blackboard.GetFloatValue("VelocityX");
            float y = blackboard.GetFloatValue("VelocityY");
            return x == 0f && y == 0f;
        }
    }

    public class Tirgger3 : IStateTrigger
    {
        public bool Check(IBlackboard blackboard)
        {
            float x = blackboard.GetFloatValue("VelocityX");
            float y = blackboard.GetFloatValue("VelocityY");
            bool leftShift = blackboard.GetBoolValue("LeftShift");
            return (x != 0f || y != 0f) && leftShift;
        }
    }

    public class Tirgger4 : IStateTrigger
    {
        public bool Check(IBlackboard blackboard)
        {
            float x = blackboard.GetFloatValue("VelocityX");
            float y = blackboard.GetFloatValue("VelocityY");
            bool leftShift = blackboard.GetBoolValue("LeftShift");
            return (x == 0f && y == 0f) || !leftShift;
        }
    }

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

    public class IdleState : ComponentState
    {
        public IdleState(MotorComponent component) : base(component)
        {
        }

        protected override void OnInit()
        {
            AddTrigger<Tirgger1, WalkState>();
        }
    }

    public class WalkState : ComponentState
    {
        public WalkState(MotorComponent component) : base(component)
        {
        }

        protected override void OnInit()
        {
            AddTrigger<Tirgger2, IdleState>();
            AddTrigger<Tirgger3, RunState>();
        }

        protected override void OnEnter()
        {
            ActorComponent ac = GetComponent<ActorComponent>();
            ac.SetBool("IsWalk", true);
        }

        protected override void OnExit()
        {
            float x = GetFloatValue("VelocityX");
            float y = GetFloatValue("VelocityY");
            bool leftShift = GetBoolValue("LeftShift");
            ActorComponent ac = GetComponent<ActorComponent>();
            ac.SetBool("IsWalk", (x != 0 || y != 0) && leftShift);
        }
    }

    public class RunState : ComponentState
    {
        public RunState(MotorComponent component) : base(component)
        {
        }

        protected override void OnInit()
        {
            AddTrigger<Tirgger4, WalkState>();
        }

        protected override void OnEnter()
        {
            ActorComponent ac = GetComponent<ActorComponent>();
            ac.SetBool("IsRun", true);
        }

        protected override void OnExit()
        {
            ActorComponent ac = GetComponent<ActorComponent>();
            ac.SetBool("IsRun", false);
        }
    }

    public class MotorComponent : ComponentBase, IFixedUpdateableComponent, ILateUpdateableComponent
    {
        private struct MoveTrace
        {
            public float dirX;
            public float dirY;
            public float x;
            public float y;
            public float z;
        }

        private class StepRecord
        {
            public bool IsValid { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public bool LeftShift { get; set; }
            public int StepCount { get; set; }
            public Vector3 LastPos { get; set; }
        }

        private enum MotorState
        {
            Idle,
            LeaderMoving,
            FollowerMoving,
        }

        private const int Gap = 200;
        private const float StepDistance = 0.002f;
        private const float WalkSpeed = 1.25f;
        private const float RunSpeed = 2.5f;

        private bool isStartUp;
        private StateMachine stateMachine;
        private ActorComponent actorComponent;

        private MotorComponent target;
        private List<MoveTrace> traces;
        private bool IsMoving => state == MotorState.LeaderMoving || state == MotorState.FollowerMoving;
        private MotorState state;
        private StepRecord step;

        void IFixedUpdateableComponent.FixedUpdate(float fixedDeltaTime)
        {
            if (!isStartUp)
            {
                return;
            }

            stateMachine.Update(fixedDeltaTime);

            if (target == null)
            {
                Move();
            }
            else
            {
                //Follow();
            }
        }

        void ILateUpdateableComponent.LateUpdate(float deltaTime)
        {
            if (!isStartUp)
            {
                return;
            }

            if (target == null)
            {
                //Move();
            }
            else
            {
                Follow();
            }
        }

        protected override void Awake()
        {
            actorComponent = GetComponent<ActorComponent>();

            traces = new List<MoveTrace>();
            step = new StepRecord();

            stateMachine = new StateMachine();
            stateMachine.AddState(new IdleState(this));
            stateMachine.AddState(new WalkState(this));
            stateMachine.AddState(new RunState(this));
        }

        public void StartUp()
        {
            actorComponent.SetAnimatorController(ActorAnimator.Normal);
            stateMachine.Run<IdleState>();
            isStartUp = true;
        }

        public void ShutDown()
        {
            isStartUp = false;
        }

        public void SetFollowTarget(MotorComponent target)
        {
            this.target = target;
        }

        public void OnInputAction(InputContext context)
        {
            InputActionDefine input = context.Input;
            switch (input)
            {
                case InputActionDefine.Move:
                case InputActionDefine.Move2:
                    SetVelocity(context.X, context.Y);
                    break;
                case InputActionDefine.LeftShift:
                    SetLeftShift(context.BoolValue);
                    break;
            }
        }

        private void SetVelocity(float x, float y)
        {
            if (x != 0f || y != 0f)
            {
                actorComponent.SetDirection(x, y);
            }

            stateMachine.SetValue("VelocityX", x);
            stateMachine.SetValue("VelocityY", y);
        }

        private void SetLeftShift(bool v)
        {
            stateMachine.SetValue("LeftShift", v);
        }

        private void Move()
        {
            BreakUpStep();

            float x = stateMachine.GetFloatValue("VelocityX");
            float y = stateMachine.GetFloatValue("VelocityY");
            if (x != 0f || y != 0f)
            {
                bool leftShift = stateMachine.GetBoolValue("LeftShift");

                float baseSpeed = leftShift ? RunSpeed : WalkSpeed;
                float extraSpeed = 0f;
                actorComponent.Velocity = (1f + extraSpeed) * baseSpeed * new Vector3(x, 0f, y);
                state = MotorState.LeaderMoving;

                RecordStep(x, y, leftShift);
            }
            else
            {
                actorComponent.Velocity = Vector3.zero;
                state = MotorState.Idle;
            }
        }

        private void BreakUpStep()
        {
            if (step.IsValid)
            {
                var pos = actorComponent.Position;
                var rx = step.X;
                var ry = step.Y;
                var last = step.LastPos;
                var dx = Util.Math.Abs(pos.x - last.x);
                var dz = Util.Math.Abs(pos.z - last.z);
                if (!(dx < StepDistance && dz < StepDistance))
                {
                    if (rx != 0f && ry == 0f)
                    {
                        var r = Util.Math.Floor(dx / StepDistance);
                        for (int i = 0; i < r; i++)
                        {
                            float nx;
                            float nz = last.z;
                            if (rx > 0f)
                            {
                                nx = last.x + StepDistance * i;
                            }
                            else
                            {
                                nx = last.x - StepDistance * i;
                            }
                            PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = nx, y = pos.y, z = nz });
                        }

                        PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = pos.x, y = pos.y, z = pos.z });
                        step.StepCount = r + 1;
                    }

                    if (ry != 0f && rx == 0f)
                    {
                        var r = Util.Math.Floor(dz / StepDistance);
                        for (int i = 0; i < r; i++)
                        {
                            float nx = last.x;
                            float nz;
                            if (ry > 0f)
                            {
                                nz = last.z + StepDistance * i;
                            }
                            else
                            {
                                nz = last.z - StepDistance * i;
                            }

                            PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = nx, y = pos.y, z = nz });
                        }
                        PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = pos.x, y = pos.y, z = pos.z });
                        step.StepCount = r + 1;
                    }

                    if (rx != 0f && ry != 0f)
                    {
                        var v = StepDistance * 0.7f;
                        var r = Util.Math.Floor(dx / v);
                        for (int i = 0; i < r; i++)
                        {
                            float nx;
                            float nz;
                            if (rx > 0f)
                            {
                                nx = last.x + v * i;
                            }
                            else
                            {
                                nx = last.x - v * i;
                            }

                            if (ry > 0f)
                            {
                                nz = last.z + v * i;
                            }
                            else
                            {
                                nz = last.z - v * i;
                            }

                            PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = nx, y = pos.y, z = nz });
                        }
                        PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = pos.x, y = pos.y, z = pos.z });
                        step.StepCount = r + 1;
                    }
                }
                else
                {
                    step.StepCount = 0;
                }

                step.IsValid = false;
            }
        }

        private void RecordStep(float x, float y, bool leftShift)
        {
            step.IsValid = true;
            step.X = x;
            step.Y = y;
            step.LeftShift = leftShift;
            step.LastPos = actorComponent.Position;
        }

        private void Follow()
        {
            if (target == null) return;

            int stepCount = target.step.StepCount;
            int tracesCount = target.traces.Count;
            if (stepCount > 0 && tracesCount > Gap)
            {
                int i = Util.Math.Min(tracesCount - Gap, stepCount);
                var traces = target.PopTraces(i);
                var trace = traces[^1];

                actorComponent.MovePosition(new Vector3(trace.x, trace.y, trace.z));
                SetVelocity(trace.dirX, trace.dirY);
                SetLeftShift(target.step.LeftShift);

                PushTrace(traces);

                step.StepCount = stepCount;
                step.LeftShift = target.step.LeftShift;
                state = MotorState.FollowerMoving;
            }
            else
            {
                if (target.IsMoving && stepCount > 0)
                {
                    return;
                }
                if (!IsMoving)
                {
                    return;
                }

                SetVelocity(0f, 0f);
                SetLeftShift(false);
                state = MotorState.Idle;
            }
        }

        private void PushTrace(MoveTrace trace)
        {
            traces.Add(trace);
        }

        private void PushTrace(List<MoveTrace> items)
        {
            foreach (var trace in items)
            {
                this.traces.Add(trace);
            }
        }

        private List<MoveTrace> PopTraces(int count)
        {
            var result = new List<MoveTrace>();
            for (int j = 0; j < count; j++)
            {
                result.Add(traces[j]);
            }
            traces.RemoveRange(0, count);
            return result;
        }
    }
}