using UnityEngine;

namespace GameFramework.View.UI
{
    public class ProgressTransitionWidget : UITransitionWidget
    {
        public override void FadeIn(float time)
        {
            container.SetActive(true);
        }

        public override void FadeOut(float time)
        {
            container.SetActive(false);
        }

        public override void Transition(float progress)
        {
            container.GetWidget<SliderWidget>().SetValue(progress);
            container.GetWidget<TextWidget>().SetText($"{progress / 1f * 100:F0}%");
            
        }
    }
}
