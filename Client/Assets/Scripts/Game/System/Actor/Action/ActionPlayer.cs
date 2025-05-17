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
        private class Command : IComparable<Command>
        {
            public bool IsExecuted => isExecuted;
            public int Priority => command.Priority;
            public float Timepoint => command.Timepoint;
            public float Duration => command.Duration;
            private bool isExecuted;
            private readonly ActionCommandBase command;

            public Command(ActionPlayer player, ActionCommandBase command) 
            {
                this.command = command;
                this.command.SetPlayer(player);
                isExecuted = false;
            }

            public void Execute() 
            {
                isExecuted = true;
                command?.Execute();
            }

            public void Complete()
            {
                command?.Complete();
            }

            public int CompareTo(Command other)
            {
                return command.Priority.CompareTo(other.command.Priority);
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
        private readonly Command[] commands;

        public ActionPlayer(int actionNameHash, ActionCommandBase[] commands) 
        {
            ActionNameHash = actionNameHash;
            state = ActionPlayerState.Idle;

            int length = commands != null ? commands.Length : 0;

            this.commands = new Command[length];
            for (int i = 0; i < length; i++) 
            {
                this.commands[i] = new Command(this, commands[i]);                
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

            for (int i = 0; i < commands.Length; i++)
            {
                Command cmd = commands[i];
                cmd.Complete();
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
            for (int i = 0; i < commands.Length; i++)
            {
                Command cmd = commands[i];
                if (cmd.Timepoint < 0f)
                {
                    continue;
                }

                if (cmd.Duration < 0f)
                {
                    duration = -1f;
                    break;
                }

                float t = cmd.Timepoint + cmd.Duration;
                duration = Math.Max(duration, t);
            }
            this.duration = duration;

            Array.Sort(commands, (a, b) => b.CompareTo(a));
        }

        private void Run() 
        {
            for (int i = 0; i < commands.Length; i++)
            {
                Command cmd = commands[i];
                if (!cmd.IsExecuted && cmd.Timepoint <= timer)
                {
                    try
                    {
                        cmd.Execute();
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
