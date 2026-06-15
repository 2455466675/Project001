using ECS;
using GameFramework.Core;
using GameFramework.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Logic
{
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
            /// <summary>
            /// 上一帧一共进行了多少步。每一步0.002f
            /// </summary>
            public int StepCount { get; set; }
            public Vector3 LastPos { get; set; }
        }

        private enum MotorState
        {
            Idle,
            Moving,
        }

        private const int Gap = 200;
        private const float StepDistance = 0.002f;
        private const float WalkSpeed = 1.25f;
        private const float RunSpeed = 2.5f;

        private bool isStartUp;
        private ActorComponent actorComponent;
        private MotorAnimatorComponent animatorComponent; //动画模块

        private MotorComponent target;
        private List<MoveTrace> traces; //把每一帧移动的距离等量拆分，后面的跟随者在下一帧时移动同等份数的距离
        private bool IsMoving => state == MotorState.Moving;
        private MotorState state;
        private StepRecord step;

        private float velocityX;
        private float velocityY;
        private bool leftShift;


        void IFixedUpdateableComponent.FixedUpdate(float fixedDeltaTime)
        {
            if (!isStartUp)
            {
                return;
            }

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
            traces = new List<MoveTrace>();
            step = new StepRecord();
        }

        protected override void Start()
        {
            actorComponent = GetComponent<ActorComponent>();
            animatorComponent = GetComponent<MotorAnimatorComponent>();
        }

        public void StartUp()
        {
            actorComponent.SetAnimatorController(ActorAnimator.Normal);

            isStartUp = true;
            animatorComponent.StartUp();
        }

        public void ShutDown()
        {
            isStartUp = false;
            animatorComponent.ShutDown();
        }

        public void SetFollowTarget(int eid)
        {
            this.target = null;
            var entity = Game.GetSystem<GameEntityFactory>().GetEntity(eid);
            if (entity == null)
            {
                return;
            }
            this.target = entity.GetComponent<MotorComponent>();
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
            velocityX = x;
            velocityY = y;
            animatorComponent.SetVelocity(x, y);
        }

        private void SetLeftShift(bool v)
        {
            leftShift = v;
            animatorComponent.SetLeftShift(v);
        }

        private void Move()
        {
            BreakUpStep();

            if (TryMove())
            {
                RecordStep(velocityX, velocityY, leftShift);
            }
        }

        /// <summary>
        /// 分解上一次成功移动和本帧的距离。每一份0.002f
        /// </summary>
        private void BreakUpStep()
        {
            if (!step.IsValid)
            {
                return;
            }
            var pos = actorComponent.GetPosition();
            var rx = step.X;
            var ry = step.Y;
            var last = step.LastPos;
            var dx = Util.Math.Abs(pos.x - last.x);
            var dz = Util.Math.Abs(pos.z - last.z);
            if (dx < StepDistance && dz < StepDistance)
            {
                //没有移动，或者撞墙了
                step.StepCount = 0;
            }
            else
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

            step.IsValid = false;
        }

        /// <summary>
        /// 尝试移动
        /// </summary>
        /// <returns></returns>
        private bool TryMove()
        {
            if (velocityX != 0f || velocityY != 0f)
            {
                float baseSpeed = leftShift ? RunSpeed : WalkSpeed;
                float extraSpeed = 0f;
                actorComponent.SetVelocity((1f + extraSpeed) * baseSpeed * new Vector3(velocityX, 0f, velocityY));
                state = MotorState.Moving;

                return true;
            }
            else
            {
                actorComponent.SetVelocity(Vector3.zero);
                state = MotorState.Idle;
                return false;
            }
        }

        /// <summary>
        /// 移动成功，记录本帧的数据，下一帧与当前数据进行比较
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
            step.LastPos = actorComponent.GetPosition();
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
                state = MotorState.Moving;
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