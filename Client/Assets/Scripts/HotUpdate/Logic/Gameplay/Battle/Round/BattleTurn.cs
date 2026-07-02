using Cysharp.Threading.Tasks;

namespace GameFramework.Logic
{
    /// <summary>
    /// 行动轮次
    /// </summary>
    public class BattleTurn : IBattlePhase
    {
        private int battleID;

        /// <summary>
        /// 行动者战斗ID
        /// </summary>
        /// <param name="battleID"></param>
        public BattleTurn(int battleID)
        {
            this.battleID = battleID;
        }

        public bool IsAlive()
        {
            //TODO 行动者是否存活
            return true;
        }

        public async UniTask Run(IBattleContext context)
        {
            // TODO 等待选择行动方式 or AI : await Decider.Decide()

            //行动开始事件

            BattleAction action = new BattleAction();
            await action.Run();

            //行动结束事件
        }
    }
}
