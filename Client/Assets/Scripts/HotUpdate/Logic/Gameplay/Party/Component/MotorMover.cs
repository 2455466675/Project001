using GameFramework.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Logic
{
    /// <summary>
    /// 运动计算：负责自身移动与对目标的跟随，并把每帧位移拆分成等量轨迹供跟随者采样。
    /// 不直接依赖动画模块，速度与疾跑变化通过事件向外通知，由组合者负责转发，保持模块解耦。
    /// </summary>
    public class MotorMover
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

        private readonly IActorComponent actor;
        private readonly List<MoveTrace> traces = new List<MoveTrace>();
        private readonly StepRecord step = new StepRecord();

        private MotorMover target;
        private MotorState state;

        private float velocityX;
        private float velocityY;
        private bool leftShift;

        /// <summary>
        /// 速度倍率增量，最终速度 = 基础速度 * (1 + extraSpeed)。预留给加速/减速等增益效果，默认 0 即原速。
        /// </summary>
        private float extraSpeed;

        /// <summary>
        /// 速度变化通知。跟随模式下速度由轨迹推导得出，外部据此驱动动画，避免运动模块反向依赖动画模块。
        /// </summary>
        public event Action<float, float> VelocityChanged;
        public event Action<bool> LeftShiftChanged;

        public bool HasTarget => target != null;
        private bool IsMoving => state == MotorState.Moving;

        public MotorMover(IActorComponent actor)
        {
            this.actor = actor;
        }

        public void SetTarget(MotorMover target)
        {
            this.target = target;
        }

        public void SetVelocity(float x, float y)
        {
            velocityX = x;
            velocityY = y;
            VelocityChanged?.Invoke(x, y);
        }

        public void SetLeftShift(bool v)
        {
            leftShift = v;
            LeftShiftChanged?.Invoke(v);
        }

        /// <summary>
        /// 设置速度倍率增量。正值加速，负值减速；下限钳制到 -1，避免出现负速度（反向移动）。
        /// </summary>
        public void SetExtraSpeed(float value)
        {
            extraSpeed = value < -1f ? -1f : value;
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (HasTarget)
            {
                return;
            }
            Move();
        }

        public void OnLateUpdate(float deltaTime)
        {
            if (!HasTarget)
            {
                return;
            }
            Follow();
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
            var pos = actor.GetPosition();
            var rx = step.X;
            var ry = step.Y;
            var last = step.LastPos;
            var dx = Util.Math.Abs(pos.x - last.x);
            var dz = Util.Math.Abs(pos.z - last.z);

            if (dx < StepDistance && dz < StepDistance)
            {
                //没有移动，或者撞墙了
                step.StepCount = 0;
                step.IsValid = false;
                return;
            }

            // 斜向时每步在两轴上各取根2的一半，保证合位移仍是一个 StepDistance
            bool diagonal = rx != 0f && ry != 0f;
            float unit = diagonal ? StepDistance * 0.7f : StepDistance;
            float stepX = rx == 0f ? 0f : (rx > 0f ? unit : -unit);
            float stepZ = ry == 0f ? 0f : (ry > 0f ? unit : -unit);

            // 以主轴的实际位移估算步数；水平/斜向看 X，垂直看 Z
            float dominant = rx != 0f ? dx : dz;
            int count = Util.Math.Floor(dominant / unit);

            for (int i = 0; i < count; i++)
            {
                PushTrace(new MoveTrace()
                {
                    dirX = rx,
                    dirY = ry,
                    x = last.x + stepX * i,
                    y = pos.y,
                    z = last.z + stepZ * i,
                });
            }

            // 最后补一帧精确终点，避免累计步长与真实位移的误差
            PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = pos.x, y = pos.y, z = pos.z });
            step.StepCount = count + 1;

            step.IsValid = false;
        }

        /// <summary>
        /// 尝试移动
        /// </summary>
        private bool TryMove()
        {
            if (velocityX != 0f || velocityY != 0f)
            {
                float speed = leftShift ? RunSpeed : WalkSpeed;
                actor.SetVelocity((1f + extraSpeed) * speed * new Vector3(velocityX, 0f, velocityY));
                state = MotorState.Moving;

                return true;
            }
            else
            {
                actor.SetVelocity(Vector3.zero);
                state = MotorState.Idle;
                return false;
            }
        }

        /// <summary>
        /// 移动成功，记录本帧的数据，下一帧与当前数据进行比较
        /// </summary>
        private void RecordStep(float x, float y, bool leftShift)
        {
            step.IsValid = true;
            step.X = x;
            step.Y = y;
            step.LeftShift = leftShift;
            step.LastPos = actor.GetPosition();
        }

        private void Follow()
        {
            if (target == null)
            {
                return;
            }

            int stepCount = target.step.StepCount;
            int tracesCount = target.traces.Count;
            if (stepCount > 0 && tracesCount > Gap)
            {
                int i = Util.Math.Min(tracesCount - Gap, stepCount);
                var items = target.PopTraces(i);
                var trace = items[^1];

                actor.MovePosition(new Vector3(trace.x, trace.y, trace.z));
                SetVelocity(trace.dirX, trace.dirY);
                SetLeftShift(target.step.LeftShift);

                PushTrace(items);
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
                traces.Add(trace);
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
