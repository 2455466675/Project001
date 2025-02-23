using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class Panel : MonoBehaviour
    {
        public UIGroup group;

        public CanvasGroup canvasGroup;

        public NavigationGroup[] groups;

        /// <summary>
        /// ÏÔÊ¾
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        /// <summary>
        /// Òþ²Ø
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            transform.SetAsFirstSibling();
        }

        /// <summary>
        /// ¾Û½¹
        /// </summary>
        public void InFocus()
        {
            canvasGroup.alpha = 1f;
        }

        /// <summary>
        /// Ê§½¹
        /// </summary>
        public void OutFocus()
        {
            canvasGroup.alpha = 0.6f;
        }

        private void OnValidate()
        {
            groups = GetComponentsInChildren<NavigationGroup>();
        }
    }
}
