using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;
using Game.Cfg;
using System.Collections.Generic;
using System;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class GameUI : MonoBehaviour, ICore
    {
        public UIRoot UIRoot {get; private set;}
        public UIGuideController UIGuideController { get; set; }

        public Camera UICamera => UIRoot != null ? UIRoot.UICamera : null;

        public Window CurrentWindow { get; private set; }
        public IGuidableGroup CurrentGroup { get; private set; }
        public IGuidable LastSelectGuidable { get; private set; }
        public IGuidable CurrSelectGuidable { get; private set; }

        public event Action<IGuidable> OnSelectGuidableChanged;

        private Dictionary<WindowId, Window> windows;
        private Dictionary<WindowId, Window> showWindows;
        private Stack<OpenWindowCmd> commands;
        
        public IEnumerator Init()
        {
            GameObject obj = GameCore.ResourceManager.LoadAsset<GameObject>(GameCore.GameInitCfg.UIRootPath);
            GameObject uiRootGo = Instantiate(obj);
            UIRoot = uiRootGo.GetComponent<UIRoot>();

            windows = new Dictionary<WindowId, Window>();
            showWindows = new Dictionary<WindowId, Window>();
            commands = new Stack<OpenWindowCmd>();

            yield return UIRoot;
        }

        public void Move(Vector2 dir)
        {
            if (CurrentGroup == null)
            {
                return;
            }

            if (dir.x > 0)
            {
                CurrentGroup.OnMoveToRight();
            }
            else if (dir.x < 0)
            {
                CurrentGroup.OnMoveToLeft();
            }

            if (dir.y > 0)
            {
                CurrentGroup.OnMoveToUp();
            }
            else if(dir.y < 0)
            {
                CurrentGroup.OnMoveToDown();
            }            
        }

        public void SelectGuidable(IGuidable guidable)
        {
            LastSelectGuidable?.OnDeselected();
            CurrSelectGuidable = guidable;
            CurrSelectGuidable?.OnSelected();
            OnSelectGuidableChanged?.Invoke(guidable);
        }
        public void Submit()
        {
            CurrSelectGuidable?.OnSubmit();
        }

        public void AddShowWindow(Window window)
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

        public void RemoveShowWindow(Window window)
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

        public void InFocusWindow(Window window)
        {
            CurrentWindow = window;
        }

        public void InFocusGroup(IGuidableGroup group)
        {
            CurrentGroup = group;
        }

        /// <summary>
        /// 后退
        /// </summary>
        public void Back()
        {
            TryBack(false);
            if (commands.Count <= 0)
            {
                ExitInner(true);
            }
        }

        /// <summary>
        /// 进入UI
        /// </summary>
        /// <param name="id"></param>
        public void Enter(WindowId id)
        {
            GameCore.StateController.SwitchModel(GameModel.UI);
            ShowGuideWindow();
            OpenWindow(id);
        }

        /// <summary>
        /// 退出UI，会强制关闭所有界面，直接退出
        /// </summary>
        public void Exit()
        {
            ExitInner(true);
        }

        /// <summary>
        /// ESC键退出，会检测命令是否支持回退，停留在最近的一个无法回退的命令
        /// </summary>
        public void Close()
        {
            ExitInner(false);
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
            else if(windType == WindType.Static)
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

        public void SelectGuidableGroup(ListViewId id)
        {
            SelectGuidableGroup(ListView.GetView(id));
        }

        public void SelectGuidableGroup(IGuidableGroup group)
        {
            if (group == null)
            {
                return;
            }

            if (commands.TryPeek(out OpenWindowCmd cmd))
            {
                cmd.Push(new SelectGuidableGroupCmd(group));
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
            Window inst = GoHelper.Instantiate<Window>(win, UIRoot.GetWinGroup(win.group));
            inst.Init(cfg);
            windows[id] = inst;
            return inst;
        }

        private void ShowGuideWindow()
        {
            if (UIGuideController != null)
            {
                return;
            }
            OpenWindow(WindowId.WinGuide);
        }

        private void HideGuideWindow()
        {
            if (UIGuideController == null)
            {
                return;
            }
            UIGuideController.Hide();
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
            HideGuideWindow();
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