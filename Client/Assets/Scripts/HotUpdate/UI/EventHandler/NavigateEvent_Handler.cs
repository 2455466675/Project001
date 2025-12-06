namespace GameFramework.UI 
{
    [GameEvent]
    public class NavigateEvent_Handler : GameEventHandlerBase<NavigateEventArgs>
    {
        public override void Invoke(NavigateEventArgs arg)
        {
            Game.GetModule<UIManager>().EnterNavigate(arg.navigationDefine);
        }
    }

    [GameEvent]
    public class NavigateBackEvent_Handler : GameEventHandlerBase<NavigateBackEventArgs>
    {
        public override void Invoke(NavigateBackEventArgs arg)
        {
            Game.GetModule<InputController>().PopCammand();
        }
    }
}