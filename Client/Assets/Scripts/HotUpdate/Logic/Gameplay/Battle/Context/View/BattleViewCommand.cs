using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameFramework.Logic
{
    public abstract class BattleViewCommand
    {
        private readonly static BattleViewCommand emptyCommand = new EmptyViewCommand();
        public static BattleViewCommand EmptyCommand => emptyCommand;

        public int Batch { get; set; }
        public abstract UniTask Play();
    }

    public class EmptyViewCommand : BattleViewCommand
    {
        public override UniTask Play()
        {
            return UniTask.CompletedTask;
        }
    }

    public class TestViewCommand : BattleViewCommand
    {
        public override async UniTask Play()
        {
            MDebug.Log("TestViewCommand Play Start!");
            await UniTask.WaitForSeconds(1f);
            MDebug.Log("TestViewCommand Play Finish!");
        }
    }
   
    public class ActionContext
    {
        public class HitResult
        {
            public int targetId; //受影响的目标
            public int value; //值
            public int effectType; // 效果类型：受伤、回血、护盾、加buff...
            public int hitType; //正常命中、暴击、miss、免疫

            public Dictionary<int, string> extArgs; //额外参数
        }

        public int CasterId;

        public int ViewActionId; //对应SO资源
        
        public List<HitResult[]> HitResults; //
    }
}
