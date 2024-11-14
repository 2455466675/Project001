namespace Game.System
{
    /// <summary>
    /// 回合开始阶段
    /// </summary>

    public class RoundStartState : BattleStepBaseState
    {
        public RoundStartState(BattleRoundController controller) : base(controller)
        {
        }

        public override BattleStep Step => BattleStep.RoundStart;

        public override void Enter()
        {
            MLog.Log("回合开始阶段");

            //1、计算行动顺序，如果是第一回合，计算当前回合和下一回合的行动顺序；否则计算下一回合的行动顺序
            //2、回合开始事件触发（buff等等）
            //3、角色开始行动
            controller.RandomList();
            controller.SwitchState(BattleStep.CharacterAction);
        }
    }
}

