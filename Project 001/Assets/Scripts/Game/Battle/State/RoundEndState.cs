namespace Game.System
{
    /// <summary>
    /// 回合结束阶段
    /// </summary>
    public class RoundEndState : BattleStepBaseState
    {
        public RoundEndState(BattleRoundController controller) : base(controller)
        {
        }

        public override BattleStep Step => BattleStep.RoundEnd;

        public override void Enter()
        {
            //1、回合结束事件触发（buff等等）
            //2、进入下一个回合            
            MLog.Log("回合结束阶段");
            controller.SwitchState(BattleStep.RoundStart);
        }
    }
}

