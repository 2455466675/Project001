using System.Collections;

namespace Game.System
{
    /// <summary>
    /// 动作生效（动画、特效、动作效果）
    /// </summary>
    public class BattleRoleActionEffect : BattleRoleActionStep
    {
        public BattleRoleActionEffect(BattleRoundController controller) : base(controller)
        {
        }

        public override IEnumerator Execute(BattleRoleActionStepArg arg)
        {
            MLog.Log("动作结算", arg.Character.Index);
            arg.State = StepState.Effect;
            yield return null;
        }
    }
}

