namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionCommand<T> : ActionCommandBase where T : ActionItemBase
    {
        protected T Item => GetActionItem<T>();
    }
}
