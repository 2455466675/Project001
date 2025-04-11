namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionCommand<T> : ActionCommandBase where T : ActionItemBase
    {
        protected T ActionItem => GetActionItem<T>();
    }
}
