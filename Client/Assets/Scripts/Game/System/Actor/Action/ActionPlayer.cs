using Cysharp.Threading.Tasks;
using System;

namespace Game.GSystem
{
    public enum ActionPlayerState 
    {
        Idle,
        Processing,
        Completed,
    }

    /// <summary>
    /// 
    /// </summary>
    public class ActionPlayer
    {
        private class Execution : IComparable<Execution>
        {
            public bool IsExecuted => isExecuted;
            public int Priority => executor.Priority;
            public float Timepoint => executor.Timepoint;
            public float Duration => executor.Duration;
            private bool isExecuted;
            private readonly ActionCommandExecutor executor;

            public Execution(ActionPlayer player, ActionCommandExecutor executor) 
            {
                this.executor = executor;
                this.executor.Player = player;
                isExecuted = false;
            }

            public void Execute() 
            {
                isExecuted = true;
                executor?.Execute();
            }

            public void Complete()
            {
                executor?.Complete();
            }

            public int CompareTo(Execution other)
            {
                return executor.Priority.CompareTo(other.executor.Priority);
            }
        }

        public int ActionNameHash { get; private set; }
        public Actor Actor { get; private set; }
        public object UserData { get; private set; }

        public bool IsIdle => state == ActionPlayerState.Idle;
        public bool IsProcessing => state == ActionPlayerState.Processing;
        public bool IsCompleted => state == ActionPlayerState.Completed;

        public UniTask Task
        {
            get
            {
                if (completionSource == null)
                {
                    completionSource = new UniTaskCompletionSource();
                    if (state == ActionPlayerState.Completed)
                    {
                        completionSource.TrySetResult();
                    }
                }
                return completionSource.Task;
            }
        }

        private UniTaskCompletionSource completionSource;

        private float duration;
        private float timer;

        private ActionPlayerState state;
        private readonly Execution[] executions;

        public ActionPlayer(int actionNameHash, ActionCommandExecutor[] executors) 
        {
            ActionNameHash = actionNameHash;
            state = ActionPlayerState.Idle;

            int length = executors != null ? executors.Length : 0;

            this.executions = new Execution[length];
            for (int i = 0; i < length; i++) 
            {
                this.executions[i] = new Execution(this, executors[i]);                
            }
        }

        public ActionHandle CreateHandle() 
        {
            ActionHandle handle = new ActionHandle(this);
            return handle;
        }

        public void Start(Actor actor, object userData) 
        {
            if (!IsIdle) return;

            state = ActionPlayerState.Processing;
            Actor = actor;
            UserData = userData;

            Init();
            Run();
            CheckComplete();
        }

        public void Update(float dt) 
        {
            if (!IsProcessing) return;
            
            Run();
            CheckComplete();
            timer += dt;
        }

        public void Complete() 
        {
            if (!IsProcessing) return;

            state = ActionPlayerState.Completed;

            for (int i = 0; i < executions.Length; i++)
            {
                Execution exe = executions[i];
                exe.Complete();
            }
            
            completionSource?.TrySetResult();
            completionSource = null;
            Actor = null;
            UserData = null;
        }

        private void Init()
        {
            timer = 0f;

            float duration = 0f;
            for (int i = 0; i < executions.Length; i++)
            {
                Execution exe = executions[i];
                if (exe.Timepoint < 0f)
                {
                    continue;
                }

                if (exe.Duration < 0f)
                {
                    duration = -1f;
                    break;
                }

                float t = exe.Timepoint + exe.Duration;
                duration = Math.Max(duration, t);
            }
            this.duration = duration;

            Array.Sort(executions, (a, b) => b.CompareTo(a));
        }

        private void Run() 
        {
            for (int i = 0; i < executions.Length; i++)
            {
                Execution exe = executions[i];
                if (!exe.IsExecuted && exe.Timepoint <= timer)
                {
                    try
                    {
                        exe.Execute();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }                    
                }
            }
        }

        private void CheckComplete() 
        {
            if (duration < 0f) 
            {
                return;
            }

            if (timer >= duration)
            {
                Complete();
            }
        }
    }
}
