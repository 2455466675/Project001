namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class EmptyAction : ActionItem<EmptyActionCommand>
    {

    }

    public class EmptyActionCommand : ActionCommand<EmptyAction> 
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
