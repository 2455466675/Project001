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
}
