namespace GameFramework.UI 
{
    [GameEvent]
    public class NavigateEvent_Handler : GameEventHandlerBase<NavigateEventArgs>
    {
        public override void Invoke(NavigateEventArgs arg)
        {
            Game.GetModule<UIManager>().Navigate(arg.navigationDefine);
        }
    }
}