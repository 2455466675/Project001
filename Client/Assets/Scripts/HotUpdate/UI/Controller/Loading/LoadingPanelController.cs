using UnityEngine;

namespace GameFramework.UI
{
    [UIPanelController(PanelDefine.LoadingPanel)]
    public class LoadingPanelController : PanelController
    {
        protected override void OnShow()
        {
            Game.Event.Register<LoadingProgressEventArgs>(OnLoadingProgress);
        }

        protected override void OnHide()
        {
            Game.Event.Unregister<LoadingProgressEventArgs>(OnLoadingProgress);
        }

        private void OnLoadingProgress(LoadingProgressEventArgs args)
        {
            float progress = Mathf.Clamp01(args.progress);
            SliderWidget slider = GetWidget<SliderWidget>();
            if (slider != null) 
            {
                slider.SetValue(progress);
            }
            TextWidget text = GetWidget<TextWidget>();
            if (text)
            {
                text.SetText(string.Format("{0:F0}%", progress * 100));
            }
        }
    }
}
