using Game.Cfg;
using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class WindowController
	{
        public Window CurrWindow { get; private set; }
        private Dictionary<WindowId, Window> windows;
        private Dictionary<WindowId, Window> showWindows;
        private Stack<OpenWindowCmd> commands;

        public WindowController()
        {
            windows = new Dictionary<WindowId, Window>();
            showWindows = new Dictionary<WindowId, Window>();
            commands = new Stack<OpenWindowCmd>();
        }
        public void InFocusWindow(Window window)
        {
            CurrWindow = window;
        }

        public void ShowWindow(Window window)
        {
            if (window == null)
            {
                return;
            }

            WindowId id = (WindowId)window.Id;
            if (showWindows.ContainsKey(id))
            {
                return;
            }
            showWindows[id] = window;
        }

        public void HideWindow(Window window)
        {
            if (window == null)
            {
                return;
            }

            WindowId id = (WindowId)window.Id;
            if (!showWindows.ContainsKey(id))
            {
                return;
            }
            showWindows.Remove(id);
        }

        public void Enter(WindowId id)
        {
            GameCore.StateController.SwitchModel(GameModel.UI);
            GameCore.UI.ShowGuideWindow();
            OpenWindow(id);
        }

        public void Exit()
        {
            ExitInner(true);
        }

        public void Close()
        {
            ExitInner(false);
        }

        public void Back()
        {
            TryBack(false);
            if (commands.Count <= 0)
            {
                ExitInner(true);
            }
        }

        public void OpenWindow(WindowId id)
        {
            Window window = GetWindowInst(id);
            if (window == null)
            {
                return;
            }

            WindType windType = window.winType;
            if (windType == WindType.Guide)
            {
                window.Show();
            }
            else if (windType == WindType.Static)
            {
                window.Show();
            }
            else
            {
                OpenWindowCmd cmd = new OpenWindowCmd(window);
                commands.Push(cmd);
                cmd.OnPush();
            }
        }

        public void SelectNavigatable(INavigatable navigatable)
        {
            if (navigatable == null)
            {
                return;
            }

            if (commands.TryPeek(out OpenWindowCmd cmd))
            {
                cmd.Push(new SelectNavigatableCmd(navigatable));
            }
            else
            {
                MLog.Error("没有依附的窗口");
            }
        }

        private Window GetWindowInst(WindowId id)
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
            if (prefab == null)
            {
                MLog.Error($"没有窗体资源:{cfg.Path}");
                return null;
            }
            Window win = prefab.GetComponent<Window>();
            Window inst = GoHelper.Instantiate<Window>(win, GameCore.UI.UIRoot.GetWinGroup(win.group));
            inst.Init(cfg);
            windows[id] = inst;
            return inst;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="ignoreUndoable">是否忽略对命令的可回退性检查</param>
        private void ExitInner(bool ignoreUndoable = false)
        {
            while (commands.Count > 0)
            {
                if (!TryBack(ignoreUndoable))
                {
                    return;
                }
            }
            GameCore.UI.HideGuideWindow();
            GameCore.StateController.SwitchModel(GameModel.SCENE);
        }

        /// <summary>
        /// 尝试回退
        /// </summary>
        /// <param name="ignoreUndoable">是否忽略对命令的可回退性检查</param>
        /// <returns></returns>
        private bool TryBack(bool ignoreUndoable)
        {
            if (commands.TryPeek(out OpenWindowCmd cmd))
            {
                if (!ignoreUndoable && !cmd.IsUndoable)
                {
                    MLog.Log("不可回退");
                    return false;
                }

                cmd.Pop();
                if (cmd.IsEmpty)
                {
                    cmd.OnPop();
                    commands.Pop();

                    if (commands.TryPeek(out cmd))
                    {
                        cmd.OnRise();
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

