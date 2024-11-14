using Game.UI;

namespace Game.System
{
    /// <summary>
    /// 战斗开始阶段
    /// </summary>
    public class BattleStartState : BattleStepBaseState
    {
        public BattleStartState(BattleRoundController controller) : base(controller)
        {
        }

        public override BattleStep Step => BattleStep.BattleStart;

        public override void Enter()
        {
            MLog.Log("战斗开始阶段");
            GameCore.UI.ShowWindow(WindowId.WinBattleInfo);
            controller.SwitchState(BattleStep.RoundStart);
        }
    }
}

