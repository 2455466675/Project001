using Cysharp.Threading.Tasks;

namespace GameFramework.Core
{
    public class ActionHandle
    {
        public UniTask Task => player.Task;
        public bool IsCompleted => player.IsCompleted;

        private readonly ActionPlayer player;

        internal ActionHandle(ActionPlayer player)
        {
            this.player = player;
        }

        public void Stop()
        {
            player?.Complete();
        }
    }
}
