using System.Collections;

namespace Game.System
{
    /// <summary>
    /// 6¡¢×´Ì¬½áÊø
    /// </summary>
	public class BattleRoleActionEnd : BattleRoleAction
    {
        public BattleRoleActionEnd(BattleRoundController controller) : base(controller)
        {
        }

        public override IEnumerator Execute(BattleRoleActionArg arg)
        {
            arg.State = StepState.End;
            controller.SwitchState(BattleStep.CharacterAction);
            yield return null;
        }
    }
}

