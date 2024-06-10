using Game.Cfg;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class Window : MonoBehaviour
    {
        public UIGroupEnum group;
        public WindowCfg Cfg { get; private set;}
        public UIController Controller { get; private set; }

        public int Id => Cfg != null ? Cfg.id : -1;
        
        public string Path => Cfg != null ? Cfg.path : string.Empty;

        public void SetCfg(WindowCfg cfg)
        {
            Cfg = cfg;
        }

        public void SetController(UIController controller)
        {
            this.Controller = controller;
        }

        public void OnOpen()
        {
            if (Controller != null) 
            {
                Controller.OnOpen();
            }
        }
    }
}