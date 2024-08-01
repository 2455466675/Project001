using Game.Cfg;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Game.UI
{
    public enum WindType 
    {
        Base    = 0,
        Normal  = 1,
        Guide   = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public class Window : MonoBehaviour
    {
        public UIGroupEnum group;
        public WindType winType;
        public CanvasGroup canvasGroup;
        public WindowCfg Cfg {get; private set;}
        [ShowInInspector]
        public UIController Controller {get; private set;}
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

        public void SetCfg(WindowCfg cfg)
        {
            Cfg = cfg;
        }

        public void SetController(UIController controller)
        {
            Controller = controller;
        }

        public void Show()
        {
            if (Controller != null)
            {
                Controller.OnShow();
            }
            canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0f;
        }

        public void Enter()
        {
            if (Controller != null)
            {
                Controller.OnEnter();
            }
        }

        public void Exit()
        {
            
        }
    }
}