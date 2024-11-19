using System.Collections;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BattleRoleActionStep
    {
        protected readonly BattleRoundController controller;

        public BattleRoleActionStep(BattleRoundController controller)
        {
            this.controller = controller;
        }

        /// <summary>
        /// 行动
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public abstract IEnumerator Execute(BattleRoleActionStepArg arg);

        /// <summary>
        /// 行动再次返回
        /// </summary>
        /// <param name="arg"></param>
        public virtual void ActionCallBack(BattleRoleActionStepArg arg) 
        {
        }
    }
}

