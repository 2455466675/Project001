namespace Game.System
{
    public enum BattleStage
    {
        /// <summary>
        /// 战斗准备阶段
        /// </summary>
        BattlePrep,
        /// <summary>
        /// 战斗开始
        /// </summary>
        BattleStart,
        /// <summary>
        /// 回合开始
        /// </summary>
        RoundStart,

        /// <summary>
        /// 角色行动
        /// </summary>
        CharacterAction,


        /// <summary>
        /// 回合结束
        /// </summary>
        RoundEnd,
        /// <summary>
        /// 战斗结束
        /// </summary>
        BattleEnd,
        /// <summary>
        /// 战斗结算阶段
        /// </summary>
        BattleClear,
        /// <summary>
        /// 游戏结束
        /// </summary>
        GameOver,
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class BattleStageBase
    {
        public abstract BattleStage Stage { get; }

        protected BattleRoundController controller;

        public BattleStageBase(BattleRoundController controller)
        {
            this.controller = controller;
        }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {

        }
    }
}

