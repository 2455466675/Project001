using System;
using System.Collections;
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
        public Window Current => showStack.Peek();
        private Dictionary<WindowId, Window> windows;
        private Stack<Window> showStack;

        public WindowSystem()
        {
            windows = new Dictionary<WindowId, Window>();
            showStack = new Stack<Window>();
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

        public void HideWindow()
        {
            if (showStack.TryPop(out Window window))
            {
                window.Hide();
            }
        }

        public bool WindowIsTop(WindowId id)
        {
            if (Current == null)
            {
                return false;
            }
            return Current.Id == (int)id;         
        }

        public void HideAll()
        {
            while (showStack.Count > 0)
            {
                if (showStack.TryPop(out Window window))
                {
                    window.Hide();
                }
            }

            showStack.Clear();
        }

        private Window GetWindowInstance(WindowId id)
        {
            if (windows.ContainsKey(id))
            {
                return windows[id];
            }

            WindowCfg cfg = GameCore.GameCfg.Find<WindowCfg>((int)id);
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

            WindowCfg cfg = GameCore.GameCfg.Find<WindowCfg>((int)id);
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
            Window inst = GoHelper.Instantiate<Window>(win, GameCore.UI.UIRoot.GetWinGroup(win.group));
            inst.Init(cfg);
            windows[(WindowId)cfg.Id] = inst;
            return inst;
        }

        private void Show(Window window)
        {
            if (window == null)
            {
                return;
            }
            if (showStack.TryPeek(out Window win) && win.Id == window.Id)
            {
                return;
            }
            showStack.Push(window);
            window.Show();
        }
    }
}

