using GameFramework.Core;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Gameplay
{
    public class CharacterAnimState : StateItemBase
    {
        protected ActorComponent Actor { get; private set; }

        public CharacterAnimState(Featrue.Component component)
        {
            Actor = component.GetComponent<ActorComponent>();
        }
    }
    public class Idle2WalkTirgger : StateTriggerBase<WalkState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            float x = blackboard.GetBlackboardFloatValue("DirX");
            float y = blackboard.GetBlackboardFloatValue("DirY");
            return x != 0 || y != 0;
        }
    }

    public class Idle2RunTirgger : StateTriggerBase<RunState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            float x = blackboard.GetBlackboardFloatValue("DirX");
            float y = blackboard.GetBlackboardFloatValue("DirY");
            return x != 0 || y != 0;
        }
    }

    public class IdleState : CharacterAnimState
    {
        public IdleState(Featrue.Component component) : base(component)
        {
        }

        protected override void OnEnter()
        {
            float x = GetBlackboardFloatValue("LastDirX");
            float y = GetBlackboardFloatValue("LastDirY");
            Actor.SetFloat("DirX", x);
            Actor.SetFloat("DirY", y);
        }
    }

    public class Walk2IdleTirgger : StateTriggerBase<IdleState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            float x = blackboard.GetBlackboardFloatValue("DirX");
            float y = blackboard.GetBlackboardFloatValue("DirY");
            return x == 0f && y == 0f;
        }
    }

    public class Walk2RunTirgger : StateTriggerBase<RunState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            float x = blackboard.GetBlackboardFloatValue("DirX");
            float y = blackboard.GetBlackboardFloatValue("DirY");
            bool leftShift = blackboard.GetBlackboardBoolValue("LeftShift");
            return (x != 0f || y != 0f) && leftShift;
        }
    }

    public class WalkState : CharacterAnimState
    {
        public WalkState(Featrue.Component component) : base(component)
        {
        }

        protected override void OnEnter()
        {
            Actor.SetBool("IsWalk", true);
        }

        protected override void OnExit()
        {
            float x = GetBlackboardFloatValue("DirX");
            float y = GetBlackboardFloatValue("DirY");
            bool leftShift = GetBlackboardBoolValue("LeftShift");
            Actor.SetBool("IsWalk", (x != 0 || y != 0) && leftShift);
        }

        protected override void OnTick()
        {
            float x = GetBlackboardFloatValue("DirX");
            float y = GetBlackboardFloatValue("DirY");
            Actor.SetFloat("DirX", x);
            Actor.SetFloat("DirY", y);
        }
    }

    public class Run2WalkTirgger : StateTriggerBase<WalkState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            float x = blackboard.GetBlackboardFloatValue("DirX");
            float y = blackboard.GetBlackboardFloatValue("DirY");
            bool leftShift = blackboard.GetBlackboardBoolValue("LeftShift");
            return (x == 0f && y == 0f) || !leftShift;
        }
    }

    public class Run2IdleTirgger : StateTriggerBase<IdleState>
    {
        public override bool Check(IBlackboard blackboard)
        {
            float x = blackboard.GetBlackboardFloatValue("DirX");
            float y = blackboard.GetBlackboardFloatValue("DirY");
            return x == 0f && y == 0f;
        }
    }

    public class RunState : CharacterAnimState
    {
        public RunState(Featrue.Component component) : base(component)
        {
        }

        protected override void OnEnter()
        {
            Actor.SetBool("IsRun", true);
        }

        protected override void OnExit()
        {
            Actor.SetBool("IsRun", false);
        }

        protected override void OnTick()
        {
            float x = GetBlackboardFloatValue("DirX");
            float y = GetBlackboardFloatValue("DirY");
            Actor.SetFloat("DirX", x);
            Actor.SetFloat("DirY", y);
        }
    }

    public class MotorComponent : Featrue.Component, IFixedUpdate
    {
        private const int Gap = 200;
        private const float StepDistance = 0.002f;

        private const float WalkSpeed = 1.25f;
        private const float RunSpeed = 2.5f;

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
        public ActorComponent Actor { get; private set; }

        private bool isStartUp;
        private StateMachine m_Machine;
        private MotorComponent m_Target;
        private List<MoveTrace> m_Traces;

        private bool IsMoving => m_State == MotorState.LeaderMoving || m_State == MotorState.FollowerMoving;
        private MotorState m_State;

        private StepRecord m_Step;

        protected override void OnInit()
        {
            m_Machine = new StateMachine();

            IdleState idleState = new IdleState(this);
            idleState.AddTrigger(new Idle2WalkTirgger());
            m_Machine.AddState(idleState);

            WalkState walkState = new WalkState(this);
            walkState.AddTrigger(new Walk2IdleTirgger());
            walkState.AddTrigger(new Walk2RunTirgger());
            m_Machine.AddState(walkState);

            RunState runState = new RunState(this);
            runState.AddTrigger(new Run2WalkTirgger());
            m_Machine.AddState(runState);

            m_Traces = new List<MoveTrace>();
            m_Step = new StepRecord();

            Actor = GetComponent<ActorComponent>();
        }
   
        public void FixedUpdate()
        {
            if (!isStartUp)
            {
                return;
            }

            m_Machine.Tick();

            if (m_Target == null)
            {
                Move();
            }
            else
            {
                Follow();
            }
        }

        public void StartUp()
        {
            Actor.SetAnimatorController(ActorAnimator.Normal);
            m_Machine.Run<IdleState>();
            isStartUp = true;
        }

        public void ShutDown()
        {
            isStartUp = false;
        }

        public void OnInputAction(InputContext context)
        {
            InputDefine input = context.Input;
            switch (input)
            {
                case InputDefine.Move:
                    SetXY(context.X, context.Y);
                    break;
                case InputDefine.LeftShift:                    
                    SetLeftShift(context.BoolValue);
                    break;
            }
        }

        public void SetFollowTarget(MotorComponent target)
        {
            m_Target = target;
        }

        private void SetXY(float x, float y)
        {
            m_Machine.SetBlackboardValue("LastDirX", m_Machine.GetBlackboardFloatValue("DirX"));
            m_Machine.SetBlackboardValue("LastDirY", m_Machine.GetBlackboardFloatValue("DirY"));
            m_Machine.SetBlackboardValue("DirX", x);
            m_Machine.SetBlackboardValue("DirY", y);
        }

        private void SetLeftShift(bool v)
        {
            m_Machine.SetBlackboardValue("LeftShift", v);
        }

        private void Move()
        {
            var ac = Actor;

            if (m_Step.IsValid)
            {
                var pos = ac.Position;
                var rx = m_Step.X;
                var ry = m_Step.Y;
                var last = m_Step.LastPos;

                var dx = Utility.Math.Abs(pos.x - last.x);
                var dz = Utility.Math.Abs(pos.z - last.z);
                if (!(dx < 0.01f && dz < 0.01f))
                {          
                    if (rx != 0f && ry == 0f)
                    {
                        var r = Utility.Math.Floor(dx / StepDistance);
                        for (int i = 0; i < r; i++)
                        {
                            float nx;
                            float nz = last.z;
                            if (rx > 0f)
                            {
                                nx = last.x + StepDistance;
                            }
                            else
                            {
                                nx = last.x - StepDistance;
                            }

                            PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = nx, y = pos.y, z = nz });
                        }

                        PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = pos.x, y = pos.y, z = pos.z });
                        m_Step.StepCount = r + 1;
                    }

                    if (ry != 0f && rx == 0f)
                    {
                        var r = Utility.Math.Floor(dz / StepDistance);
                        for (int i = 0; i < r; i++)
                        {
                            float nx = last.x;
                            float nz;
                            if (ry > 0f)
                            {
                                nz = last.z + StepDistance;
                            }
                            else
                            {
                                nz = last.z - StepDistance;
                            }

                            PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = nx, y = pos.y, z = nz });
                        }
                        PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = pos.x, y = pos.y, z = pos.z });
                        m_Step.StepCount = r + 1;
                    }

                    if (rx != 0f && ry != 0f)
                    {
                        var v = StepDistance * 0.7f;
                        var r = Utility.Math.Floor(dx / v);
                        for (int i = 0; i < r; i++)
                        {
                            float nx;
                            float nz;
                            if (rx > 0f)
                            {
                                nx = last.x + v;
                            }
                            else
                            {
                                nx = last.x - v;
                            }

                            if (ry > 0f)
                            {
                                nz = last.z + v;
                            }
                            else
                            {
                                nz = last.z - v;
                            }

                            PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = nx, y = pos.y, z = nz });
                        }
                        PushTrace(new MoveTrace() { dirX = rx, dirY = ry, x = pos.x, y = pos.y, z = pos.z });
                        m_Step.StepCount = r + 1;
                    }
                }
                else
                {
                    m_Step.StepCount = 0;
                }

                m_Step.IsValid = false;
            }

            float x = m_Machine.GetBlackboardFloatValue("DirX");
            float y = m_Machine.GetBlackboardFloatValue("DirY");
            
            if (x != 0f || y != 0f)
            {
                bool leftShift = m_Machine.GetBlackboardBoolValue("LeftShift");

                float baseSpeed = leftShift ? RunSpeed : WalkSpeed;
                float extraSpeed = 0f;
                ac.Velocity = (1f + extraSpeed) * baseSpeed * new Vector3(x, 0f, y);
                
                m_State = MotorState.LeaderMoving;
                m_Step.IsValid = true;
                m_Step.X = x; 
                m_Step.Y = y;
                m_Step.LeftShift = leftShift;
                m_Step.LastPos = ac.Position;
            }
            else
            {
                ac.Velocity = Vector3.zero;
                m_State = MotorState.Idle;
            }
        }

        private void Follow() 
        {
            if (m_Target == null) return;

            int stepCount = m_Target.m_Step.StepCount;
            int tracesCount = m_Target.m_Traces.Count;
            if (stepCount > 0 && tracesCount > Gap)
            {           
                int i = Utility.Math.Min(tracesCount - Gap, stepCount);

                var traces = m_Target.PopTraces(i);
                var trace = traces[^1];

                float dirX = trace.dirX;
                float dirY = trace.dirY;                
                var ac = Actor;
                var target = new Vector3(trace.x, trace.y, trace.z);
                ac.MovePosition(target);
                SetXY(dirX, dirY);
                SetLeftShift(m_Target.m_Step.LeftShift);

                PushTrace(traces);
         
                m_State = MotorState.FollowerMoving;
                m_Step.StepCount = stepCount;
                m_Step.LeftShift = m_Target.m_Step.LeftShift;
            }
            else
            {
                if (m_Target.IsMoving && stepCount > 0)
                {
                    return;
                }
                if (!IsMoving)
                {
                    return;
                }
       
                SetXY(0f, 0f);
                SetLeftShift(false);
                m_State = MotorState.Idle;
            }
        }

        private void PushTrace(MoveTrace trace)
        {
            m_Traces.Add(trace);
        }

        private void PushTrace(List<MoveTrace> traces)
        {
            foreach (var trace in traces)
            {
                m_Traces.Add(trace);
            }
        }

        private List<MoveTrace> PopTraces(int count)
        {
            var traces = new List<MoveTrace>();
            for (int j = 0; j < count; j++)
            {
                traces.Add(m_Traces[j]);
            }
            m_Traces.RemoveRange(0, count);
            return traces;
        }
    }
}
