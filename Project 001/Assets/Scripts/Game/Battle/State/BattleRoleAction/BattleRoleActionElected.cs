using System.Collections;

namespace Game.System
{
    /// <summary>
    /// 确定行动角色
    /// </summary>
    public class BattleRoleActionElected : BattleRoleAction
    {
        public BattleRoleActionElected(BattleRoundController controller) : base(controller)
        {
        }

        public override IEnumerator Execute(BattleRoleActionArg arg)
        {
            MLog.Log("CharacterElected");
            arg.State = StepState.Effect;
            arg.Character = controller.GetCharacter();
            yield return null;
        }
    }
}

