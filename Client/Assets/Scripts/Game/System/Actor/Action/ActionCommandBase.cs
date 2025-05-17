namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionCommandBase
    {
        public int Priority => item != null ? item.Priority : 0;
        public float Timepoint => item != null ? item.Timepoint : -1f;
        public float Duration => item != null ? item.Duration : 0f;

        private ActionItemBase item;
        protected ActionPlayer Player { get; private set; }

        public void SetPlayer(ActionPlayer player) 
        {
            Player = player;
        }

        public void SetActionItem(ActionItemBase item) 
        {
            this.item = item;
        }

        public void Execute()
        {
            OnExecute();
        }

        public void Complete()
        {
            OnComplete();
            item = null;
            Player = null;
        }

        protected T GetActionItem<T>() where T : ActionItemBase 
        {
            if (item == null) 
            {
                return default;
            }
            else
            {
                return item as T;
            }
        }

        protected virtual void OnExecute() { }
        protected virtual void OnComplete() { }
    }
}
