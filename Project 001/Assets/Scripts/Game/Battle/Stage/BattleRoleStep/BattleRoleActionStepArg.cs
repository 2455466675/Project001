namespace Game.System
{
    public enum StepState
    {
        None,
        /// <summary>
        /// 确定行动角色
        /// </summary>
        Elected,
        /// <summary>
        /// 角色就位
        /// </summary>
        TakePlace,
        /// <summary>
        /// 动作选择
        /// </summary>
        Choice,
        /// <summary>
        /// 动作生效
        /// </summary>
        Effect,
        /// <summary>
        /// 动作结算
        /// </summary>
        Clearing,
        /// <summary>
        /// 状态结束
        /// </summary>
        End,
    }

    public class BattleRoleActionStepArg
    {
        /// <summary>
        /// 当前角色
        /// </summary>
        public BattleRole Character { get; set; }
        /// <summary>
        /// 锁定
        /// </summary>
        public bool IsLocked { get; set; }
        /// <summary>
        /// 当前步骤
        /// </summary>
        public StepState State { get; set; }
        public object Data { get; set; }

        public BattleRoleActionStepArg()
        {
            Reset();
        }

        /// <summary>
        /// 重置参数
        /// </summary>
        public void Reset()
        {
            Character = null;
            IsLocked = false;
            State = StepState.None;
            Data = null;
        }
    }
}

