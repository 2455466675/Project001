using GameFramework.Core;

namespace GameFramework.View.UI
{
    [UIPanelController(Utility.GameDefine.PanelDefine.GameTransitionPanel)]
    public class GameTransitionPanelController : PanelController
    {
        protected override string AssetPath => "Assets/Bundles/UI/Prefabs/Panel/GameTransitionPanel";

        protected override void OnShow()
        {
            Game.Message.Subscribe<GameTransitionFadeInMessage>(FadeIn);
            Game.Message.Subscribe<GameTransitionFadeOutMessage>(FadeOut);
            Game.Message.Subscribe<GameTransitionProgressMessage>(Transition);
        }

        protected override void OnHide()
        {
            Game.Message.Unsubscribe<GameTransitionFadeInMessage>(FadeIn);
            Game.Message.Unsubscribe<GameTransitionFadeOutMessage>(FadeOut);
            Game.Message.Unsubscribe<GameTransitionProgressMessage>(Transition);
        }

        private void FadeIn(GameTransitionFadeInMessage message)
        {
            var widget = GetWidget<UITransitionWidget>(message.transitionType.ToString());
            if (widget != null)
            {
                widget.FadeIn(message.fadeInTime);
            }
        }

        private void FadeOut(GameTransitionFadeOutMessage message)
        {
            var widget = GetWidget<UITransitionWidget>(message.transitionType.ToString());
            if (widget != null)
            {
                widget.FadeOut(message.fadeOutTime);
            }
        }

        private void Transition(GameTransitionProgressMessage message)
        {
            var widget = GetWidget<UITransitionWidget>(message.transitionType.ToString());
            if (widget != null)
            {
                widget.Transition(message.progress);
            }
        }
    }
}