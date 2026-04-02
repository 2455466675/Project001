using Cysharp.Threading.Tasks;
using GameFramework.Logic;
using GameFramework.Utility.GameDefine;
using UnityEngine;

namespace GameFramework.View.UI
{
    [UIPanelController(PanelDefine.BattleLoadingPanel)]
    public class BattleLoadingPanelController : PanelController
    {
        protected override void OnShow()
        {
            Transitional();
        }

        protected override void OnHide()
        {

        }

        private async void Transitional()
        {
            var widget = GetWidget<ImageTransitionalWidget>();
            float m = 0.5f;
            float t = 0f;
            widget.SetValue(1f);
            while (t < m)
            {
                t += Time.deltaTime;
                widget.SetValue(1 - t / m);
                await UniTask.Yield();
            }
            await UniTask.Yield();
            widget.SetValue(-0.1f);
            await UniTask.WaitForSeconds(0.5f);
            while (t > 0f)
            {
                t -= Time.deltaTime * 1.5f;
                widget.SetValue(1 - t / m);
                await UniTask.Yield();
            }

            await UniTask.Yield();
            widget.SetValue(1f);

            Game.Message.SendMessage(new UIPanelMessage() { isVisible = false, panel = PanelDefine.BattleLoadingPanel });
        }
    }
}