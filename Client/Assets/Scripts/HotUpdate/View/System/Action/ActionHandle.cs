using Cysharp.Threading.Tasks;

namespace GameFramework.View.Action
{
    public class ActionHandle
    {
        public static readonly ActionHandle Completed = new ActionHandle(UniTask.CompletedTask);

        private readonly UniTask task;

        public ActionHandle(UniTask task)
        {
            this.task = task;
        }

        public UniTask.Awaiter GetAwaiter() => task.GetAwaiter();
    }
}
