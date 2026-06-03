using ECS;
using FSM;
using GameFramework.Core;
using GameFramework.Logic;
using GameFramework.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.View
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
        public IdleState(MotorProjectionComponent component) : base(component)
        {
        }

        protected override void OnInit()
        {
            AddTrigger<Tirgger1, WalkState>();
        }
    }

    public class WalkState : ComponentState
    {
        public WalkState(MotorProjectionComponent component) : base(component)
        {
        }

        protected override void OnInit()
        {
            AddTrigger<Tirgger2, IdleState>();
            AddTrigger<Tirgger3, RunState>();
        }

        protected override void OnEnter()
        {
            var ac = GetComponent<ActorProjectionComponent>();
            ac.SetBool("IsWalk", true);
        }

        protected override void OnExit()
        {
            float x = GetFloatValue("VelocityX");
            float y = GetFloatValue("VelocityY");
            bool leftShift = GetBoolValue("LeftShift");
            var ac = GetComponent<ActorProjectionComponent>();
            ac.SetBool("IsWalk", (x != 0 || y != 0) && leftShift);
        }
    }

    public class RunState : ComponentState
    {
        public RunState(MotorProjectionComponent component) : base(component)
        {
        }

        protected override void OnInit()
        {
            AddTrigger<Tirgger4, WalkState>();
        }

        protected override void OnEnter()
        {
            var ac = GetComponent<ActorProjectionComponent>();
            ac.SetBool("IsRun", true);
        }

        protected override void OnExit()
        {
            var ac = GetComponent<ActorProjectionComponent>();
            ac.SetBool("IsRun", false);
        }
    }

    public class MotorProjectionComponent : ComponentBase, IProjection, IMotorComponent, IFixedUpdateableComponent, ILateUpdateableComponent
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
            /// <summary>
            /// 上一帧一共进行了多少步。每一步0.002f
            /// </summary>
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
        private ActorProjectionComponent actorComponent;

        private MotorProjectionComponent target;
        private List<MoveTrace> traces; //把每一帧移动的距离等量拆分，后面的跟随者在下一帧时移动同等份数的距离
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

        void IMotorComponent.StartUp()
        {
            actorComponent.SetAnimatorController(ActorAnimator.Normal);
            stateMachine.Run<IdleState>();
            isStartUp = true;
        }

        void IMotorComponent.ShutDown()
        {
            isStartUp = false;
        }

        void IMotorComponent.SetFollowTarget(int eid)
        {
            this.target = null;
            var projectionEntity = Game.GetSystem<GameProjector>().GetProjection(eid);
            if (projectionEntity == null)
            {
                return;
            }
            this.target = projectionEntity.GetComponent<MotorProjectionComponent>();
        }

        void IMotorComponent.OnInputAction(InputContext context)
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

        protected override void Awake()
        {
            traces = new List<MoveTrace>();
            step = new StepRecord();

            stateMachine = new StateMachine();
            stateMachine.AddState(new IdleState(this));
            stateMachine.AddState(new WalkState(this));
            stateMachine.AddState(new RunState(this));
        }

        protected override void Start()
        {
            actorComponent = GetComponent<ActorProjectionComponent>();
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

        /// <summary>
        /// 分解上一帧移动的距离。每一份0.002f
        /// </summary>
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
                    if (rx != 0f && ry == 0f) //水平方向移动
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

                    if (ry != 0f && rx == 0f) //垂直方向移动
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

                    if (rx != 0f && ry != 0f)   //斜方向移动
                    {
                        var v = StepDistance * 0.7f;    //根2的一半
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
                    //没有移动，或者撞墙了
                    step.StepCount = 0;
                }

                step.IsValid = false;
            }
        }

        /// <summary>
        /// 记录本帧的数据，下一帧与当前数据进行比较
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="leftShift"></param>
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
