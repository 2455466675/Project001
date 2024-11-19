using Game.UI;

namespace Game.System
{
    /// <summary>
    /// 战斗开始阶段
    /// </summary>
    public class BattleStartStage : BattleStageBase
    {
        public BattleStartStage(BattleRoundController controller) : base(controller)
        {
        }

        public override BattleStage Stage => BattleStage.BattleStart;

        public override void Enter()
        {
            MLog.Log("战斗开始阶段");
            GameCore.UI.ShowWindow(WindowId.WinBattleInfo);
            controller.SwitchState(BattleStage.RoundStart);
        }
    }
}

