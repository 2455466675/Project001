namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionItem<T> : ActionItemBase where T : ActionCommandBase, new()
    {
        public override ActionCommandBase CreateCommand() 
        {
            T cmd = new();
            cmd.SetActionItem(this);
            return cmd;
        }
    }
}
