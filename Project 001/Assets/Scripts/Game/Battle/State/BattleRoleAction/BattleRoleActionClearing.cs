using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 5、动作结算
    /// </summary>
    public class BattleRoleActionClearing : BattleRoleAction
    {
        public BattleRoleActionClearing(BattleRoundController controller) : base(controller)
        {
        }

        //1、战斗是否结束
        // 胜利 =》战斗结算状态
        // 失败 =》游戏结束状态

        //2、行动结束
        //   行动结束事件
        //   玩家角色恢复站位
        //   回合是否结束
        //   结束 =》回合结束状态
        //   没有 =》下一个角色行动
        public override IEnumerator Execute(BattleRoleActionArg arg)
        {
            MLog.Log("CharacterActionClearing");
            arg.State = StepState.Clearing;
            BattleRole character = arg.Character;

            if (character.FightCharacterType == BattleRoleType.Enemy)
            {
                MLog.Log("电脑-动作结算", character.Index);
                //controller.SwitchState(FightStep.CharacterAction);
                yield break;
            }
            else
            {
                bool wait = true;
                character.Actor.transform.DOLocalMove(new Vector3(0f, 0f, 0), 0.2f).onComplete += () =>
                {
                    wait = false;
                };

                while (wait)
                {
                    yield return null;
                }
                //controller.SwitchState(FightStep.CharacterAction);
            }
        }
    }
}

