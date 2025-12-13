using System;
using Cysharp.Threading.Tasks;

namespace GameFramework.Core
{
    internal class ActionPlayer
    {
        private enum ActionPlayerState
        {
            Idle,
            Processing,
            Completed,
        }

        private class Execution : IComparable<Execution>
        {
            public bool IsExecuted => isExecuted;
            public int Priority => command.priority;
            public float Timepoint => command.timepoint;
            public float Duration => command.duration;
            private bool isExecuted;
            private readonly ActionCommand command;

            public Execution(ActionPlayer player, ActionCommand command)
            {
                this.command = command;

                isExecuted = false;          
            }

            public void Reset(ActionData data)
            {
                isExecuted = false;
                command.Init(data);
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

            public int CompareTo(Execution other)
            {
                return Priority.CompareTo(other.Priority);
            }
        }

        private ActionPlayerState state;
        internal bool IsIdle => state == ActionPlayerState.Idle;
        internal bool IsProcessing => state == ActionPlayerState.Processing;
        internal bool IsCompleted => state == ActionPlayerState.Completed;

        private Execution[] executions;     
        private ActionData actionData;
        private float duration;
        private float timer;

        private UniTaskCompletionSource completionSource;
        internal UniTask Task
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

        internal ActionPlayer(ActionCommand[] commands)
        {
            state = ActionPlayerState.Idle;

            int length = commands != null ? commands.Length : 0;

            executions = new Execution[length];
            for (int i = 0; i < length; i++)
            {
                executions[i] = new Execution(this, commands[i]);
            }
        }

        internal ActionHandle CreateHandle()
        {
            ActionHandle handle = new ActionHandle(this);
            return handle;
        }

        internal void Reset(ActionData data)
        {
            state = ActionPlayerState.Idle;
            this.actionData = data;
            Init();
        }

        internal void Start()
        {
            if (!IsIdle) return;

            state = ActionPlayerState.Processing;
            Run();
            CheckComplete();
        }

        internal void Update(float dt)
        {
            if (!IsProcessing) return;

            Run();
            CheckComplete();
            timer += dt;
        }

        internal void Complete()
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
            actionData = null;
        }

        private void Init()
        {
            timer = 0f;

            float duration = 0f;
            for (int i = 0; i < executions.Length; i++)
            {
                Execution exe = executions[i];
                exe.Reset(actionData);

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
                        MDebug.Error(e.ToString());
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
