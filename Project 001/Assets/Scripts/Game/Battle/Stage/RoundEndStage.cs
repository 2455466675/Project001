namespace Game.System
{
    /// <summary>
    /// 回合结束阶段
    /// </summary>
    public class RoundEndStage : BattleStageBase
    {
        public RoundEndStage(BattleRoundController controller) : base(controller)
        {
        }

        public override BattleStage Stage => BattleStage.RoundEnd;

        public override void Enter()
        {
            //1、回合结束事件触发（buff等等）
            //2、进入下一个回合            
            MLog.Log("回合结束阶段");
            controller.SwitchState(BattleStage.RoundStart);
        }
    }
}

