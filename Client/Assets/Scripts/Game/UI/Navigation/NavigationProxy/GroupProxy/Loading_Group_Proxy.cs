using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    [GroupProxy(NavigationGroupDefine.Loading_Group)]
    public class Loading_Group_Proxy : NavigationGroupProxy
    {
        public override void Show()
        {
            base.Show();
            Game.Event.Register<SceneLoadingProgressEventArgs>(OnProgressUpdate);
        }

        public override void Hide()
        {
            base.Hide();
            Game.Event.Unregister<SceneLoadingProgressEventArgs>(OnProgressUpdate);
        }

        private void OnProgressUpdate(SceneLoadingProgressEventArgs arg) 
        {
            TextView textView = GetView<TextView>();
            if (textView != null) 
            {
                textView.SetTextByStr($"{Mathf.Min(100, Mathf.FloorToInt(arg.progress * 100))}%", 3);
            }

            SliderView sliderView = GetView<SliderView>();
            if (sliderView != null) 
            {
                sliderView.SetValue(arg.progress);
            }
        }
    }
}
