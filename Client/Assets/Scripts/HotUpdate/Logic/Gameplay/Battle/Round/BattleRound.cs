using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameFramework.Logic
{
    /// <summary>
    /// 战斗回合
    /// </summary>
    public class BattleRound : IBattlePhase
    {
        public async UniTask Run(IBattleContext context)
        {
            //回合开始事件

            //构建行动队列
            context.Schedule.Rebuild();

            while (context.Schedule.Dequeue(out int battleID))
            {
                BattleTurn turn = new BattleTurn(battleID);

                if (!turn.IsAlive())
                {
                    continue;
                }

                await turn.Run(context);
            }

            //回合结束事件
        }
    }
}
