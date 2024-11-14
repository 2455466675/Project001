namespace Game.System
{
    /// <summary>
    /// 战斗准备阶段
    /// </summary>
    public class BattlePrepState : BattleStepBaseState
    {
        public BattlePrepState(BattleRoundController controller) : base(controller)
        {
        }

        public override BattleStep Step => BattleStep.BattlePrep;
        public override void Enter()
        {
            MLog.Log("战斗准备阶段");
            controller.SwitchState(BattleStep.BattleStart);
        }
    }
}

