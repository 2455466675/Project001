namespace GameFramework.UI 
{
    [GameEvent]
    public class UIPanelEvent_Handler : GameEventHandlerBase<UIPanelEventArgs>
    {
        public override void Invoke(UIPanelEventArgs arg)
        {
            if (arg.isShow) 
            {
                Game.GetModule<UIManager>().ShowPanel(arg.panelDefine);
            }
            else
            {
                Game.GetModule<UIManager>().HidePanel(arg.panelDefine);
            }            
        }
    }
}