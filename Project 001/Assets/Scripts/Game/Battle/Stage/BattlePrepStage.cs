namespace Game.System
{
    /// <summary>
    /// 战斗准备阶段
    /// </summary>
    public class BattlePrepStage : BattleStageBase
    {
        public BattlePrepStage(BattleRoundController controller) : base(controller)
        {
        }

        public override BattleStage Stage => BattleStage.BattlePrep;
        public override void Enter()
        {
            MLog.Log("战斗准备阶段");
            controller.SwitchState(BattleStage.BattleStart);
        }
    }
}

