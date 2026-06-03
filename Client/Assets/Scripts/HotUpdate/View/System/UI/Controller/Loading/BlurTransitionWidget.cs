using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFramework.View.UI
{
    public class BlurTransitionWidget : UITransitionWidget
    {
        public override void FadeIn(float time)
        {
            FadeInTransitional(time);
        }

        public override void FadeOut(float time)
        {
            FadeOutTransitional(time);
        }

        private async void FadeInTransitional(float time)
        {
            container.SetActive(true);
            var widget = container.GetWidget<ImageTransitionalWidget>();
            float m = time;
            float t = 0f;
            widget.SetValue(1f);
            while (t < m)
            {
                t += Time.deltaTime;
                widget.SetValue(1 - t / m);
                await UniTask.Yield();
            }
            widget.SetValue(-0.1f);         
        }

        private async void FadeOutTransitional(float time)
        {
            var widget = container.GetWidget<ImageTransitionalWidget>();
            float m = time;
            float t = time;
            widget.SetValue(-0.1f);
            while (t > 0f)
            {
                t -= Time.deltaTime;
                widget.SetValue(1 - t / m);
                await UniTask.Yield();
            }
            widget.SetValue(1f);
            container.SetActive(false);
        }
    }
}