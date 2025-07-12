namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class EmptyCommand : ActionCommand<EmptyCommandExecutor>
    {

    }

    public class EmptyCommandExecutor : ActionCommandExecutor<EmptyCommand> 
    {
        protected override void OnExecute()
        {
            MLog.Log("EmptyAction Execute");        
        }

        protected override void OnComplete()
        {
            MLog.Log("EmptyAction OnComplete");
        }
    }
}
