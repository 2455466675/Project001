namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionCommandBase
    {
        private ActionItemBase item;
        public int Priority => item != null ? item.Priority : 0;
        public float Timepoint => item != null ? item.Timepoint : -1f;
        public float Duration => item != null ? item.Duration : 0f;

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
