using Cysharp.Threading.Tasks;

namespace GameFramework.Logic
{
    public class BattleFlow : IBattlePhase
    {
        public async UniTask Run(IBattleContext context)
        {
            MDebug.Log("BattleFlow Start");
            // 战斗开始事件
            context.EventHub.Fire(BattleEventType.BattleStart, context, new BattleEventArgs());

            while (!context.CheckFinish())
            {
                BattleRound round = new BattleRound();
                await round.Run(context);
            }

            // 战斗结束事件
            MDebug.Log("BattleFlow Finish");
            context.EventHub.Fire(BattleEventType.BattleFinish, context, new BattleEventArgs());
        }
    }
}
