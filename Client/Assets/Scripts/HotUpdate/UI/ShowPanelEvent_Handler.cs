namespace GameFramework.UI 
{
    [GameEvent]
    public class ShowPanelEvent_Handler : GameEventHandlerBase<ShowPanelEventArgs>
    {
        public override void Invoke(ShowPanelEventArgs arg)
        {
            Game.GetModule<UIManager>().ShowPanel(arg.panelDefine);
        }
    }
}