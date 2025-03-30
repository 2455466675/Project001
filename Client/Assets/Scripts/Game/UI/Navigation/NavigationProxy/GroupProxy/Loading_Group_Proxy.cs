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
            GameWorld.Root.GetComponent<EventComponent>().Register<SceneLoadingProgress>(OnProgressUpdate);
        }

        public override void Hide()
        {
            base.Hide();
            GameWorld.Root.GetComponent<EventComponent>().Unregister<SceneLoadingProgress>(OnProgressUpdate);
        }

        private void OnProgressUpdate(SceneLoadingProgress arg) 
        {
            TextView textView = groupView.GetView<TextView>();
            if (textView != null) 
            {
                textView.SetTextByStr($"{Mathf.Min(100, Mathf.FloorToInt(arg.progress * 100))}%", 3);
            }

            SliderView sliderView = groupView.GetView<SliderView>();
            if (sliderView != null) 
            {
                sliderView.SetValue(arg.progress);
            }
        }
    }
}
