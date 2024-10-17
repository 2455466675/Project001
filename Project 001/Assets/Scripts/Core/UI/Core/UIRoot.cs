using Game.Cfg;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UIRoot : MonoBehaviour
    {
        public Camera UICamera;
        public List<WindowGroup> groups;
          
        public void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public WindowGroup GetWinGroup(UIGroup group)
        {
            WindowGroup g = groups.Find(g => g.group == group);
            if (g == null)
            {
                return null;
            }
            else
            {
                return g;
            }            
        }

        public Window InstantiateWindow(Window prefab, WindowCfg cfg)
        {
            if (prefab == null)
            {
                return null;
            }

            WindowGroup group = GetWinGroup(prefab.group);
            if (group == null)
            {
                return null;
            }

            return group.InstantiateWindow(prefab, cfg);
        }

        public void Show(Window window)
        {
            WindowGroup group = GetWinGroup(window.group);
            if (group == null)
            {
                return;
            }
            group.Show(window.Id);
        }

        public void Hide(Window window)
        {
            WindowGroup group = GetWinGroup(window.group);
            if (group == null)
            {
                return;
            }
            group.Hide(window.Id);
        }
    }
}