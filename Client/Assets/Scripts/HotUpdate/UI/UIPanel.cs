using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UI 
{
    public class UIPanel : UIWidgetContainer
    {
        [SerializeField]
        private NavigationView[] navigation;

        [SerializeField]
        private CanvasGroup canvasGroup;

        public NavigationView[] GetNavigationViews()
        {
            return navigation;
        }

        public void Show() 
        {
            if (canvasGroup != null) 
            {
                canvasGroup.alpha = 1f;
            }
            transform.SetAsLastSibling();
        }

        public void Hide() 
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }          
        }

#if UNITY_EDITOR

        [Button("Init")]
        protected override void InitEditor()
        {
            base.InitEditor();

            void Fun(Transform tf, List<NavigationView> views)
            {
                var r = tf.GetComponents<NavigationView>();
                if (r != null && r.Length > 0)
                {
                    views.AddRange(r);
                    return;
                }

                int childCount = tf.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    var child = tf.GetChild(i);
                    Fun(child, views);
                }
            }

            List<NavigationView> views = new List<NavigationView>();
            Fun(transform, views);

            navigation = views.ToArray();

            canvasGroup = GetComponent<CanvasGroup>();
        }

#endif
    }
}