namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionCommandExecutor
    {
        public int Priority => command != null ? command.Priority : 0;
        public float Timepoint => command != null ? command.Timepoint : -1f;
        public float Duration => command != null ? command.Duration : 0f;
        public ActionPlayer Player { get; set; }

        private ActionCommand command;

        public void SetCommand(ActionCommand command) 
        {
            this.command = command;
        }

        public void Execute()
        {
            OnExecute();
        }

        public void Complete()
        {
            OnComplete();
            command = null;
            Player = null;
        }

        protected T GetCommand<T>() where T : ActionCommand 
        {
            if (command == null) 
            {
                return default;
            }
            else
            {
                return command as T;
            }
        }

        protected virtual void OnExecute() { }
        protected virtual void OnComplete() { }
    }

    public class ActionCommandExecutor<T> : ActionCommandExecutor where T : ActionCommand
    {
        protected T Command => GetCommand<T>();
    }
}
