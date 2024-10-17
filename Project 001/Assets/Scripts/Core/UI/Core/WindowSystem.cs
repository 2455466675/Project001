using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Cfg;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class WindowSystem
	{
        public Window Current => showList.Count > 0 ? showList[^1] : null;
        private Dictionary<WindowId, Window> windows;
        private List<Window> showList;

        public WindowSystem()
        {
            windows = new Dictionary<WindowId, Window>();
            showList = new List<Window>();
        }

        public bool WindowIsTop(WindowId id)
        {
            if (Current == null)
            {
                return false;
            }
            return Current.Id == (int)id;
        }

        public bool WindowIsShow(WindowId id)
        {
            return showList.Find(w => w.Id == (int)id) != null;
        }

        public Window ShowWindow(WindowId id)
        {
            Window window = GetWindowInstance(id);
            Show(window);
            return window;
        }

        public async UniTask<Window> ShowWindowAsync(WindowId id)
        {
            Window window = await GetWindowInstanceAsync(id);  
            Show(window);
            return window;
        }

        public void HideWindow(WindowId id)
        {
            Window window = showList.Find(w => w.Id == (int)id);
            if (window == null)
            {
                return;
            }
            Hide(window);
            showList.Remove(window);
        }

        public void HideAll()
        {
            for (int i = showList.Count - 1; i >= 0; i--)
            {
                Hide(showList[i]);
            }

            showList.Clear();
        }

        private Window GetWindowInstance(WindowId id)
        {
            if (windows.ContainsKey(id))
            {
                return windows[id];
            }

            WindowCfg cfg = GameCore.Cfg.Find<WindowCfg>((int)id);
            if (cfg == null)
            {
                MLog.Error($"没有窗体配置:{id}");
                return null;
            }
            GameObject prefab = GameCore.ResourceManager.LoadAsset<GameObject>(cfg.Path);
            return GenerateWindow(prefab, cfg);
        }

        private async UniTask<Window> GetWindowInstanceAsync(WindowId id)
        {
            if (windows.ContainsKey(id))
            {
                return windows[id];
            }

            WindowCfg cfg = GameCore.Cfg.Find<WindowCfg>((int)id);
            if (cfg == null)
            {
                MLog.Error($"没有窗体配置:{id}");
                return null;
            }
            GameObject prefab = await GameCore.ResourceManager.LoadAssetAsync<GameObject>(cfg.Path);
            return GenerateWindow(prefab, cfg);
        }

        private Window GenerateWindow(GameObject prefab, WindowCfg cfg)
        {
            if (prefab == null)
            {
                MLog.Error($"没有窗体资源:{cfg.Path}");
                return null;
            }
            Window win = prefab.GetComponent<Window>();
            Window inst = GameCore.UI.UIRoot.InstantiateWindow(win, cfg);
            windows[(WindowId)cfg.Id] = inst;
            return inst;
        }

        private void Show(Window window)
        {
            if (window == null)
            {
                return;
            }

            if (showList.Find(w => w.Id == window.Id) != null)
            {
                return;
            }

            showList.Add(window);
            GameCore.UI.UIRoot.Show(window);
        }

        private void Hide(Window window)
        {
            GameCore.UI.UIRoot.Hide(window);
        }
    }
}

