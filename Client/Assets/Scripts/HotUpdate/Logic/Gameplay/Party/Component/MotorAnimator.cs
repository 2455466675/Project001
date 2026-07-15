using FSM;

namespace GameFramework.Logic
{
    /// <summary>
    /// 动画状态管理：依据速度与疾跑输入驱动 Idle/Walk/Run 状态机。
    /// 状态过渡为 Idle &lt;=&gt; Walk &lt;=&gt; Run，Idle 与 Run 之间无直接过渡，
    /// 因此每个状态只维护自身的动画参数，避免互相猜测目标状态。
    /// </summary>
    public class MotorAnimator
    {
        private abstract class AnimatorState : StateBase
        {
            protected IPuppet Animator { get; }

            protected AnimatorState(IPuppet animator)
            {
                Animator = animator;
            }
        }

        private class MovingTrigger : IStateTrigger
        {
            public bool Check(IBlackboard blackboard)
            {
                float x = blackboard.GetFloatValue("VelocityX");
                float y = blackboard.GetFloatValue("VelocityY");
                return x != 0f || y != 0f;
            }
        }

        private class StoppedTrigger : IStateTrigger
        {
            public bool Check(IBlackboard blackboard)
            {
                float x = blackboard.GetFloatValue("VelocityX");
                float y = blackboard.GetFloatValue("VelocityY");
                return x == 0f && y == 0f;
            }
        }

        private class SprintTrigger : IStateTrigger
        {
            public bool Check(IBlackboard blackboard)
            {
                float x = blackboard.GetFloatValue("VelocityX");
                float y = blackboard.GetFloatValue("VelocityY");
                bool leftShift = blackboard.GetBoolValue("LeftShift");
                return (x != 0f || y != 0f) && leftShift;
            }
        }

        private class SlowDownTrigger : IStateTrigger
        {
            public bool Check(IBlackboard blackboard)
            {
                float x = blackboard.GetFloatValue("VelocityX");
                float y = blackboard.GetFloatValue("VelocityY");
                bool leftShift = blackboard.GetBoolValue("LeftShift");
                return (x == 0f && y == 0f) || !leftShift;
            }
        }

        private class IdleState : AnimatorState
        {
            public IdleState(IPuppet animator) : base(animator)
            {
            }

            protected override void OnInit()
            {
                AddTrigger<MovingTrigger, WalkState>();
            }

            protected override void OnEnter()
            {
                // 仅在回到 Idle 时关闭 IsWalk，IsWalk 的生命周期由 Walk 进入开启、Idle 进入关闭
                Animator.SetAnimatorValue("IsWalk", false);
            }
        }

        private class WalkState : AnimatorState
        {
            public WalkState(IPuppet animator) : base(animator)
            {
            }

            protected override void OnInit()
            {
                AddTrigger<StoppedTrigger, IdleState>();
                AddTrigger<SprintTrigger, RunState>();
            }

            protected override void OnEnter()
            {
                Animator.SetAnimatorValue("IsWalk", true);
            }

            // 退出时不动 IsWalk：去 Idle 由 IdleState 关闭，去 Run 时 IsWalk 需保持 true
        }

        private class RunState : AnimatorState
        {
            public RunState(IPuppet animator) : base(animator)
            {
            }

            protected override void OnInit()
            {
                AddTrigger<SlowDownTrigger, WalkState>();
            }

            protected override void OnEnter()
            {
                Animator.SetAnimatorValue("IsRun", true);
            }

            protected override void OnExit()
            {
                Animator.SetAnimatorValue("IsRun", false);
            }
        }

        private readonly StateMachine stateMachine;
        private readonly IPuppet animator;

        private float dirX;
        private float dirY;

        public MotorAnimator(IPuppet animator)
        {
            this.animator = animator;
            stateMachine = new StateMachine();
            stateMachine.AddState(new IdleState(animator));
            stateMachine.AddState(new WalkState(animator));
            stateMachine.AddState(new RunState(animator));
        }

        public void StartUp()
        {
            stateMachine.Run<IdleState>();
            RefreshDirection();
        }

        public void Tick(float deltaTime)
        {
            stateMachine.Update(deltaTime);
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
            animator.SetAnimatorValue("DirX", dirX);
            animator.SetAnimatorValue("DirY", dirY);
        }
    }
}
