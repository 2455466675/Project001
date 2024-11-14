using System.Collections;

namespace Game.System
{
    /// <summary>
    /// 动作选择（电脑的是AI功能）
    /// </summary>
    public class BattleRoleChoiceAction : BattleRoleAction
    {
        public BattleRoleChoiceAction(BattleRoundController controller) : base(controller)
        {
        }

        public override IEnumerator Execute(BattleRoleActionArg arg)
        {
            MLog.Log("CharacterChoiceAction");
            arg.State = StepState.Choice;
            BattleRole character = arg.Character;

            if (character.FightCharacterType == BattleRoleType.Enemy)
            {
                MLog.Log("电脑-动作选择", character.Index);
                yield break;
            }
            else
            {
                arg.IsLocked = true;

                GameCore.UI.Enter(UI.ListName.BattleActionList);

                while (arg.IsLocked)
                {
                    yield return null;
                }

                MLog.Log("选择确定", arg.Character.Index);
            }
        }

        public override void ActionCallBack(BattleRoleActionArg arg)
        {
            GameCore.UI.Exit();
            arg.IsLocked = false;
        }
    }
}

