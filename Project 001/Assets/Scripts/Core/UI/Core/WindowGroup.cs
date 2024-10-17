using Game.Cfg;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class WindowGroup : MonoBehaviour
	{
        public UIGroup group;
        private Dictionary<int, Window> windows;

        private void Awake()
        {
            windows = new Dictionary<int, Window>();
        }

        public Window InstantiateWindow(Window prefab, WindowCfg cfg)
        {
            if (prefab == null)
            {
                return null;
            }

            int id = cfg.Id;
            if (windows.ContainsKey(id))
            {
                return windows[id];
            }
            else
            {
                Window inst = GoHelper.Instantiate<Window>(prefab, transform);
                inst.Init(cfg);
                windows[id] = inst;
                return inst;                
            }
        }

        public void Show(WindowId windowId)
        {
            Show((int)windowId);
        }

        public void Show(int windowId)
        {
            int id = windowId;
            if (!windows.ContainsKey(id))
            {
                return;
            }
            Window window = windows[id];
            window.transform.SetAsLastSibling();
            window.Show();
        }

        public void Hide(WindowId windowId)
        {           
            Hide((int)windowId);
        }

        public void Hide(int windowId)
        {
            int id = windowId;
            if (!windows.ContainsKey(id))
            {
                return;
            }
            Window window = windows[id];
            window.transform.SetAsFirstSibling();
            window.Hide();
        }
    }
}

