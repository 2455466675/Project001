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

    [GameEvent]
    public class NavigateClearEvent_Handler : GameEventHandlerBase<NavigateClearEventArgs>
    {
        public override void Invoke(NavigateClearEventArgs arg)
        {
            Game.GetModule<InputController>().PopAllCammand();
        }
    }
}