using UnityEngine;
using Navigation;
using Cysharp.Threading.Tasks;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public abstract class ListProxy : INavigationElement
    {
        private NavigationList list;
        private Window parent;
        public NavigationList List => list;
        public Window Parent => parent;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid => list != null && parent != null;
        /// <summary>
        /// 列表是否已准备就绪
        /// </summary>
        public bool IsReady => (!list.IsValid) || (list.IsValid && list.UpateTime > 0);

        public abstract ListName Name { get; }
        public abstract WindowId WindowId { get; }

        public void LoadWindow()
        {
            Window window = GameCore.UI.ShowWindow(WindowId);
            parent = window;
            list = window.Find(Name);
        }

        public async UniTask LoadWindowAsync()
        {
            Window window = await GameCore.UI.ShowWindowAsync(WindowId);
            parent = window;
            list = window.Find(Name);
        }

        public void Exit()
        {
            list.Exit();
        }

        public bool InFocus(params int[] indexs)
        {
            return list.InFocus(indexs);
        }

        public bool Move(Vector2 dir)
        {
            return list.Move(dir);
        }

        public bool OutFocus()
        {
            return list.OutFocus();
        }

        public bool Refocus()
        {
            return list.Refocus();
        }

        /// <summary>
        /// 该列表的选中是否能取消
        /// </summary>
        /// <returns>是否能取消</returns>
        public virtual bool IsUndoable()
        {
            return true;
        }
    }
}

