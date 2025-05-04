using Cysharp.Threading.Tasks;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionHandle
    {
        public UniTask Task => player.Task;
        public bool IsCompleted => player.IsCompleted;

        private readonly ActionPlayer player;

        public ActionHandle(ActionPlayer player) 
        {
            this.player = player;
        }
    }
}
