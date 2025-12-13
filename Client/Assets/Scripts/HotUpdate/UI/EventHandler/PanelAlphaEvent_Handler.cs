using GameFramework.Core;

namespace GameFramework.UI
{
    [GameEvent]
    public class PanelAlphaEvent_Handler : GameEventHandlerBase<PanelAlphaEventArgs>
    {
        public override void Invoke(PanelAlphaEventArgs arg)
        {
            var controller = Game.GetModule<UIManager>().GetPanelController(arg.panel);          
            controller?.SetAlpha(arg.alpha);
        }
    }
}