using UnityEngine;
using Navigation;
using Cysharp.Threading.Tasks;
using System;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public abstract class ListProxy : INavigationElement
    {
        protected NavigationList list;
        protected Window parent;
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

        private event Action<GuidableItem[]> OnSubmitEvent;

        public abstract ListName Name { get; }
        public abstract WindowId WindowId { get; }

        /// <summary>
        /// 代理数据准备，要确保在这里将list实例获取到
        /// </summary>
        /// <returns></returns>
        public abstract UniTask Precondition();

        /// <summary>
        /// 注册点击事件
        /// </summary>
        /// <param name="action"></param>
        public void Register(Action<GuidableItem[]> action)
        {
            if (action == null)
            {
                return;
            }
            OnSubmitEvent += action;
        }

        public virtual void OnSubmit()
        {
            list.OnSubmit();
            OnSubmitEvent?.Invoke(list.SelectedItems);
        }

        public virtual void Exit()
        {
            list.Exit();
            OnSubmitEvent = null;
        }

        public virtual bool InFocus(params int[] indexs)
        {
            return list.InFocus(indexs);
        }

        public virtual bool Move(Vector2 dir)
        {
            return list.Move(dir);
        }

        public virtual bool OutFocus()
        {
            return list.OutFocus();
        }

        public virtual bool Refocus()
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

        protected async UniTask LoadWindowAsync()
        {
            Window window = await GameCore.UI.ShowWindowAsync(WindowId);
            parent = window;
            list = window.Find(Name);
        }
    }
}

