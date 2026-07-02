using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameFramework.Logic
{
    public enum Phase
    {
        BeginBattle, //开始 => CharacterShowing
        CharacterShowing, // 角色入场 => ActionQueueUp

        ActionQueueUp, //行动队列排序 => StartTurn
        StartTurn, //回合开始 => ActionQueueChecker （结束检查点 => BattleFinish）
        ActionQueueChecker, // (出队) => CharacterInPosition or（空队）=> EndTurn
        CharacterInPosition, // 就位 => CharacterAction
        CharacterAction, //行动 => ActionEffect
        ActionEffect, //行为生效 => CharacterReturnPosition （结束检查点 => BattleFinish）
        CharacterReturnPosition, // 归位 => ActionQueueChecker （结束检查点 => BattleFinish）
        EndTurn, //回合结束 => ActionQueueUp （结束检查点 => BattleFinish）

        BattleFinish, //结束（结算） Break
    }


    public abstract class RoundPhase
    {
        public abstract Phase Phase { get; }
        public abstract UniTask Run();
        public abstract Phase Next();
    }

    public class BattleRound
    {
        private Dictionary<Phase, RoundPhase> phases;

        public BattleRound()
        {
            phases = new Dictionary<Phase, RoundPhase>();
            //TODO 创建阶段
        }

        public void Start()
        {
            var phase = phases[Phase.BeginBattle];

            //循环阶段
            phase.Run();

            var nextPhase = phases[phase.Next()]; // Loop
            nextPhase.Run();            
        }
    }
}
