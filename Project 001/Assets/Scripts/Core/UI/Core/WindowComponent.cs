using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public abstract class WindowComponent : MonoBehaviour
	{
        public Window window;

        protected virtual void Awake()
        {
            if (window == null)
            {
                window = GetComponentInParent<Window>();
            }
            if (window != null)
            {
                window.OnShowEvent += OnShow;
                window.OnHideEvent += OnHide;
                window.OnInFocusEvent += OnInFocus;
                window.OnOutFocusEvent += OnOutFocus;
            }
        }

        protected virtual void OnDestroy()
        {
            if (window != null)
            {
                window.OnShowEvent -= OnShow;
                window.OnHideEvent -= OnHide;
                window.OnInFocusEvent -= OnInFocus;
                window.OnOutFocusEvent -= OnOutFocus;
            }
        }

        protected abstract void OnShow();

        protected abstract void OnHide();

        protected abstract void OnInFocus();

        protected abstract void OnOutFocus();
    }
}

