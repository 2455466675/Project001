using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 角色就位（电脑没有）
    /// </summary>
    public class BattleRoleTakePlace : BattleRoleAction
    {
        public BattleRoleTakePlace(BattleRoundController controller) : base(controller)
        {
        }

        public override IEnumerator Execute(BattleRoleActionArg arg)
        {
            MLog.Log("CharacterTakePlace");
            arg.State = StepState.TakePlace;
            BattleRole character = arg.Character;

            if (character.FightCharacterType == BattleRoleType.Enemy)
            {
                MLog.Log("电脑-角色就位", character.Index);
                yield break;
            }

            bool wait = true;

            Vector3 p = character.Actor.transform.InverseTransformPoint(new Vector3(1.5f, -0.5f, 0));

            character.Actor.transform.DOLocalMove(p, 0.2f).onComplete += () =>
            {
                wait = false;
            };

            while (wait)
            {
                yield return null;
            }
        }
    }
}

