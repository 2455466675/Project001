using System.Collections;

namespace Game.System
{
    /// <summary>
    /// 6¡¢×´Ì¬½áÊø
    /// </summary>
	public class BattleRoleActionEnd : BattleRoleActionStep
    {
        public BattleRoleActionEnd(BattleRoundController controller) : base(controller)
        {
        }

        public override IEnumerator Execute(BattleRoleActionStepArg arg)
        {
            arg.State = StepState.End;
            controller.SwitchState(BattleStage.CharacterAction);
            yield return null;
        }
    }
}

