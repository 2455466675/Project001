using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;
using Game.Cfg;
using System.Collections.Generic;
using System;

namespace Game.UI
{

    public interface INavigationController 
    {
        void Move(Vector2 dir);
        void Submit();
    }

    /// <summary>
    /// 
    /// </summary>
    public class GameUI : MonoBehaviour, ICore
    {
        public UIRoot UIRoot {get; private set;}
        public Camera UICamera => UIRoot != null ? UIRoot.UICamera : null;

        public event Action<IGuidable[]> OnSelectGuidableChanged
        {
            add
            {
                if (value != null)
                {
                    navigationController.OnSelectGuidableChanged += value;
                }
            }
            remove
            {
                if (value != null)
                {
                    navigationController.OnSelectGuidableChanged -= value;
                }
            }
        }

        private UIGuideController UIGuideController;
        private NavigationController navigationController;
        private WindowController windowController;

        public IEnumerator Init()
        {
            GameObject obj = GameCore.ResourceManager.LoadAsset<GameObject>(GameCore.GameInitCfg.UIRootPath);
            GameObject uiRootGo = Instantiate(obj);
            UIRoot = uiRootGo.GetComponent<UIRoot>();

            navigationController = new NavigationController();
            windowController = new WindowController();
            yield return UIRoot;
        }

        public void SetUIGuideController(UIGuideController controller)
        {
            UIGuideController = controller;
        }

        public void Move(Vector2 dir)
        {
            navigationController.Move(dir);
        }

        public void SelectGuidable(params IGuidable[] guidables)
        {
            navigationController.SelectGuidable(guidables);
        }
        public void Submit()
        {
            navigationController.Submit();
        }

        public void InFocusGroup(INavigatable group)
        {
            navigationController.InFocusNavigatable(group);
        }

        public void ShowWindow(Window window)
        {
            windowController.ShowWindow(window);
        }

        public void HideWindow(Window window)
        {
            windowController.HideWindow(window);
        }

        public void InFocusWindow(Window window)
        {
            windowController.InFocusWindow(window);
        }

        /// <summary>
        /// 进入UI
        /// </summary>
        /// <param name="id"></param>
        public void Enter(WindowId id)
        {
            windowController.Enter(id);
        }

        /// <summary>
        /// 退出UI，会强制关闭所有界面，直接退出
        /// </summary>
        public void Exit()
        {
            windowController.Exit();
        }

        /// <summary>
        /// ESC键退出，会检测命令是否支持回退，停留在最近的一个无法回退的命令
        /// </summary>
        public void Close()
        {
            windowController.Close();
        }

        /// <summary>
        /// 后退
        /// </summary>
        public void Back()
        {
            windowController.Back();
        }

        public void OpenWindow(WindowId id)
        {
            windowController.OpenWindow(id);
        }

        public void SelectNavigatable(ListViewId id)
        {
            windowController.SelectNavigatable(ListView.GetView(id));
        }

        public void SelectNavigatable(INavigatable navigatable)
        {
            windowController.SelectNavigatable(navigatable);
        }

        public void ShowGuideWindow()
        {
            windowController.OpenWindow(WindowId.WinGuide);
        }

        public void HideGuideWindow()
        {
            UIGuideController.Hide();
        }    
    }
}