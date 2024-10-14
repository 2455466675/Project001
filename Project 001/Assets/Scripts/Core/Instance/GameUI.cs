using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;
using Game.Cfg;
using System.Collections.Generic;
using System;
using Navigation;
using Cysharp.Threading.Tasks;

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

        public Window TopWindow => windowSystem.Current;

        public NavigationSystem navigationSystem;
        private WindowSystem windowSystem;

        public IEnumerator Init()
        {
            GameObject obj = GameCore.ResourceManager.LoadAsset<GameObject>(GameCore.GameInitCfg.UIRootPath);
            GameObject uiRootGo = Instantiate(obj);
            UIRoot = uiRootGo.GetComponent<UIRoot>();

            navigationSystem = new NavigationSystem();
            windowSystem = new WindowSystem();
            yield return UIRoot;
        }

        public void Move(Vector2 dir)
        {       
            navigationSystem.Move(dir);
        }

        public void Select(params GuidableItem[] items)
        {
            navigationSystem.Select(items);
        }

        /// <summary>
        /// 点击
        /// </summary>
        public void Submit()
        {
            navigationSystem.Submit();
        }

        /// <summary>
        /// 后退
        /// </summary>
        public void Back()
        {
            navigationSystem.Back();
        }

        /// <summary>
        /// 进入UI
        /// </summary>
        /// <param name="listName"></param>
        public void Enter(ListName listName)
        {
            navigationSystem.Enter(listName);
        }

        /// <summary>
        /// 退出UI，会强制关闭所有界面，直接退出
        /// </summary>
        public void Exit()
        {
            navigationSystem.Exit();
            HideAll();
        }

        /// <summary>
        /// ESC键退出，会检测命令是否支持回退，停留在最近的一个无法回退的命令
        /// </summary>
        public void Close()
        {

        }

        public Window ShowWindow(WindowId id)
        {
            return windowSystem.ShowWindow(id);
        }

        public async UniTask<Window> ShowWindowAsync(WindowId id)
        {
            return await windowSystem.ShowWindowAsync(id);
        }

        public void HideWindow()
        {
            windowSystem.HideWindow();
        }

        public void HideAll()
        {
            windowSystem.HideAll();
        }

        public bool WindowIsTop(WindowId id)
        {
            return windowSystem.WindowIsTop(id);
        }
    }
}