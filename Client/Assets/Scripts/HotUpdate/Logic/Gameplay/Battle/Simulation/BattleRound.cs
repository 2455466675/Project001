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
            MDebug.Log("BattleRound Start");
            context.EventHub.Fire(BattleEventType.RoundStart, context, new BattleEventArgs());

            //构建行动队列
            context.Schedule.Rebuild(context);

            while (context.Schedule.Dequeue(out int battleID))
            {
                BattleTurn turn = new BattleTurn(battleID);

                if (!turn.IsAlive())
                {
                    continue;
                }

                await turn.Run(context);

                if (context.CheckFinish())
                {
                    return;
                }
            }

            //回合结束事件
            MDebug.Log("BattleRound Finish");
            context.EventHub.Fire(BattleEventType.RoundFinish, context, new BattleEventArgs());
        }
    }
}
