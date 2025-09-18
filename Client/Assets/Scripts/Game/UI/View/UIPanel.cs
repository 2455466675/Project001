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
    }
}