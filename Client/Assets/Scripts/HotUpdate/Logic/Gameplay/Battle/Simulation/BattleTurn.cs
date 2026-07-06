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
            return true;
        }

        public async UniTask Run(IBattleContext context)
        {
                        
            context.EventHub.Fire(BattleEventType.TurnDecideBefor, context, new BattleEventArgs() { SourceID = battleID });

            // TODO await Decider.Decide() 产生BattleAction
            MDebug.Log($"{battleID} : 等待决策");
            await UniTask.WaitForSeconds(3f);

            context.EventHub.Fire(BattleEventType.TurnDecideAfter, context, new BattleEventArgs() { SourceID = battleID });

            MDebug.Log($"{battleID} : 做出决策");

            //行动开始事件
            context.EventHub.Fire(BattleEventType.TurnActionBefor, context, new BattleEventArgs() { SourceID = battleID });

            MDebug.Log($"{battleID} : 行动开始");
            await context.Projector.Flush(); // TODO 播放行动动画

            BattleAction action = new BattleAction();
            action.Execute(context);
            context.Resolver.ResolveAll(context);

            await context.Projector.Flush();

            MDebug.Log($"{battleID} : 行动结束");
            //行动结束事件
            context.EventHub.Fire(BattleEventType.TurnActionAfter, context, new BattleEventArgs() { SourceID = battleID });

        }
    }
}
