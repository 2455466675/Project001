using ECS;
using GameFramework.Core;

namespace GameFramework.Logic
{
    /// <summary>
    /// 运动组件：仅做组合与协调，自身不含运动或动画逻辑。
    /// 运动计算交由 <see cref="MotorMover"/>，动画状态管理交由 <see cref="MotorAnimator"/>，
    /// 运动模块产出的速度通过事件转发给动画模块，两者互不直接依赖。
    /// </summary>
    public class MotorComponent : ComponentBase, IFixedUpdateableComponent, ILateUpdateableComponent
    {
        private bool isStartUp;
        private ActorComponent actorComponent;
        private MotorMover mover;
        private MotorAnimator animator;

        void IFixedUpdateableComponent.FixedUpdate(float fixedDeltaTime)
        {
            if (!isStartUp)
            {
                return;
            }

            mover.OnFixedUpdate(fixedDeltaTime);
            animator.Tick(fixedDeltaTime);
        }

        void ILateUpdateableComponent.LateUpdate(float deltaTime)
        {
            if (!isStartUp)
            {
                return;
            }

            mover.OnLateUpdate(deltaTime);
        }

        protected override void Start()
        {
            actorComponent = GetComponent<ActorComponent>();

            mover = new MotorMover(actorComponent);
            animator = new MotorAnimator(actorComponent);

            // 运动模块是速度的唯一来源（输入或跟随推导），动画模块作为消费者订阅其变化
            mover.VelocityChanged += animator.SetVelocity;
            mover.LeftShiftChanged += animator.SetLeftShift;
        }

        public void StartUp()
        {
            actorComponent.SetAnimatorController(ActorAnimator.Normal);

            isStartUp = true;
            animator.StartUp();
        }

        public void ShutDown()
        {
            isStartUp = false;
        }

        /// <summary>
        /// 设置移动速度倍率增量，用于加速/减速等增益。正值加速，负值减速。
        /// </summary>
        public void SetExtraSpeed(float value)
        {
            mover.SetExtraSpeed(value);
        }

        public void SetFollowTarget(int eid)
        {
            mover.SetTarget(null);

            var entity = Game.GetSystem<GameEntityFactory>().GetEntity(eid);
            if (entity == null)
            {
                return;
            }

            var targetMotor = entity.GetComponent<MotorComponent>();
            mover.SetTarget(targetMotor.mover);
        }

        public void OnInputAction(InputContext context)
        {
            InputActionDefine input = context.Input;
            switch (input)
            {
                case InputActionDefine.Move:
                case InputActionDefine.Move2:
                    mover.SetVelocity(context.X, context.Y);
                    break;
                case InputActionDefine.LeftShift:
                    mover.SetLeftShift(context.BoolValue);
                    break;
            }
        }
    }
}
