using Game.Cfg;
using Sirenix.OdinInspector;
using UnityEngine;
using Navigation;

namespace Game.UI
{
    public enum WindType 
    {
        Static  = 0,
        Normal  = 1,
    }

    /// <summary>
    /// 
    /// </summary>
    public class Window : NavigationPanel
    {
        public UIGroup group;
        public WindType winType;
        public CanvasGroup canvasGroup;

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
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        public void InFocus()
        {
            
        }

        public  void OutFocus()
        {
            
        }        
    }
}