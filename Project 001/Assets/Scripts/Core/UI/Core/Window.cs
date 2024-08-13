using Game.Cfg;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace Game.UI
{
    public enum WindType 
    {
        Static  = 0,
        Normal  = 1,
        Guide   = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public class Window : MonoBehaviour
    {
        public UIGroup group;
        public WindType winType;
        public CanvasGroup canvasGroup;

        public event Action OnShowEvent;
        public event Action OnHideEvent;
        public event Action OnInFocusEvent;
        public event Action OnOutFocusEvent;

        public WindowCfg Cfg {get; private set;}
        [ShowInInspector]
        public int Id => Cfg != null ? Cfg.Id : -1;
        [ShowInInspector]
        public string Path => Cfg != null ? Cfg.Path : string.Empty;

        public void Awake()
        {
            if(!TryGetComponent(out canvasGroup))
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public void Init(WindowCfg cfg)
        {
            Cfg = cfg;
        }

        public void Show()
        {
            canvasGroup.alpha = 1f;
            OnShowEvent?.Invoke();
            GameCore.UI.AddShowWindow(this);
        }

        public void Hide()
        {
            canvasGroup.alpha = 0f;
            OnHideEvent?.Invoke();
            GameCore.UI.RemoveShowWindow(this);
        }

        public void InFocus()
        {
            OnInFocusEvent?.Invoke();
            GameCore.UI.InFocusWindow(this);
        }

        public void OutFocus()
        {
            OnOutFocusEvent?.Invoke();
        }        
    }
}