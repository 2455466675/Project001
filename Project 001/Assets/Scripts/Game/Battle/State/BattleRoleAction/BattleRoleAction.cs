using System.Collections;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BattleRoleAction
    {
        protected readonly BattleRoundController controller;

        public BattleRoleAction(BattleRoundController controller)
        {
            this.controller = controller;
        }

        /// <summary>
        /// 行动
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public abstract IEnumerator Execute(BattleRoleActionArg arg);

        /// <summary>
        /// 行动再次返回
        /// </summary>
        /// <param name="arg"></param>
        public virtual void ActionCallBack(BattleRoleActionArg arg) 
        {
        }
    }
}

