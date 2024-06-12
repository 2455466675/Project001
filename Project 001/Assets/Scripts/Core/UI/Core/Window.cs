using Game.Cfg;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class Window : MonoBehaviour
    {
        public UIGroupEnum group;
        public CanvasGroup canvasGroup;
        public WindowCfg Cfg { get; private set;}
        public UIController Controller { get; private set; }

        public int Id => Cfg != null ? Cfg.id : -1;
        
        public string Path => Cfg != null ? Cfg.path : string.Empty;

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

        public void Show()
        {
            canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0f;
        }
    }
}