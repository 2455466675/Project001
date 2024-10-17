using System.Collections;
using UnityEngine;
using Game.Core;
using Navigation;
using Cysharp.Threading.Tasks;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class GameUI : MonoBehaviour, ICore
    {        
        public UIRoot UIRoot {get; private set;}
        public Camera UICamera => UIRoot != null ? UIRoot.UICamera : null;

        public Window TopWindow => windowSystem.Current;

        private WindowSystem windowSystem;
        private NavigationSystem navigationSystem;

        public IEnumerator Init()
        {
            GameObject obj = GameCore.ResourceManager.LoadAsset<GameObject>(GameCore.GameInitCfg.UIRootPath);
            GameObject uiRootGo = Instantiate(obj);
            UIRoot = uiRootGo.GetComponent<UIRoot>();

            navigationSystem = new NavigationSystem();
            windowSystem = new WindowSystem();
            yield return UIRoot;
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
        /// 程序内调用，退出UI，会强制关闭所有界面，直接退出
        /// </summary>
        public void Exit()
        {
            HideAll();
            navigationSystem.Exit();
        }

        /// <summary>
        /// （WASD、方向键）选择UI
        /// </summary>
        /// <param name="dir">方向</param>
        public void Move(Vector2 dir)
        {
            navigationSystem.Move(dir);
        }

        /// <summary>
        /// （左键、Enter、空格）点击
        /// </summary>
        public void Submit()
        {
            navigationSystem.Submit();
        }

        /// <summary>
        /// （C键、右键）后退
        /// </summary>
        public void Back()
        {
            navigationSystem.Back();
        }

        /// <summary>
        /// （ESC键）退出，会停留在最近的一个无法回退的列表
        /// </summary>
        public void Close()
        {
            navigationSystem.Close();
        }

        public void Select(params GuidableItem[] items)
        {
            navigationSystem.Select(items);
        }

        public Window ShowWindow(WindowId id)
        {
            return windowSystem.ShowWindow(id);
        }

        public async UniTask<Window> ShowWindowAsync(WindowId id)
        {
            return await windowSystem.ShowWindowAsync(id);
        }

        public void HideWindow(WindowId id)
        {
            windowSystem.HideWindow(id);
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