using System.Collections;

namespace Game.System
{
    /// <summary>
    /// 角色行动阶段
    /// </summary>
    public class RoleActionStage : BattleStageBase
    {
        /// <summary>
        /// 确定行动角色
        /// </summary>
        private readonly BattleRoleActionStep characterElected;
        /// <summary>
        /// 角色就位
        /// </summary>
        private readonly BattleRoleActionStep characterTakePlace;
        /// <summary>
        /// 动作选择
        /// </summary>
        private readonly BattleRoleActionStep characterChoiceAction;
        /// <summary>
        /// 动作生效
        /// </summary>
        private readonly BattleRoleActionStep characterActionEffect;
        /// <summary>
        /// 动作结算
        /// </summary>
        private readonly BattleRoleActionStep characterActionClearing;
        /// <summary>
        /// 状态结束
        /// </summary>
        private readonly BattleRoleActionStep characterActionEnd;
        /// <summary>
        /// 状态参数
        /// </summary>
        private readonly BattleRoleActionStepArg characterActionArg;

        public RoleActionStage(BattleRoundController controller) : base(controller)
        {
            characterElected = new BattleRoleActionElected(controller);
            characterTakePlace = new BattleRoleTakePlace(controller);
            characterChoiceAction = new BattleRoleChoiceAction(controller);
            characterActionEffect = new BattleRoleActionEffect(controller);
            characterActionClearing = new BattleRoleActionClearing(controller);
            characterActionEnd = new BattleRoleActionEnd(controller);

            characterActionArg = new BattleRoleActionStepArg();
        }

        public override BattleStage Stage => BattleStage.CharacterAction;

        public override void Enter()
        {
            GameCore.Coroutine.StartCo(Execute());
        }

        /// <summary>
        /// 行动确定
        /// </summary>
        public void ActionDetermine(object data)
        {
            characterActionArg.Data = data;

            switch (characterActionArg.State) 
            {
                case StepState.Choice:
                    characterChoiceAction.ActionCallBack(characterActionArg);
                    break;
                default:
                    MLog.Error("暂未实现的操作:" + characterActionArg.State.ToString());
                    break;
            }
        }

        private IEnumerator Execute()
        {
            MLog.Log("角色行动阶段-开始");

            characterActionArg.Reset();

            yield return characterElected.Execute(characterActionArg);
            yield return characterTakePlace.Execute(characterActionArg);
            yield return characterChoiceAction.Execute(characterActionArg);
            yield return characterActionEffect.Execute(characterActionArg);
            yield return characterActionClearing.Execute(characterActionArg);

            MLog.Log("角色行动阶段-结束", characterActionArg.Character.Index);

            yield return characterActionEnd.Execute(characterActionArg);            
        }
    }
}

