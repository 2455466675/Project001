using System;
using UnityEngine;
using Game.UI;

namespace Navigation
{
    public class SelectChangedEventArgs
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccesss { get; private set; }
        /// <summary>
        /// 被选中的元素的索引列表
        /// </summary>
        public int[] Index { get; private set; }
        /// <summary>
        /// 被选中的元素
        /// </summary>
        public GuidableItem[] Items { get; private set; }
        public SelectChangedEventArgs(bool isSuccess, int[] index, GuidableItem[] items)
        {
            IsSuccesss = isSuccess;
            Index = index;
            Items = items;
        }
    }

    public class IndexChangedEventArgs
    {
        /// <summary>
        /// 起始索引
        /// </summary>
        public int MinIndex { get; private set; }
        /// <summary>
        /// 结束索引
        /// </summary>
        public int MaxIndex { get; private set; }
        /// <summary>
        /// 起始索引与结束索引之间的元素
        /// </summary>
        public GuidableItem[] Items { get; private set; }
        public IndexChangedEventArgs(int minIndex, int maxIndex, GuidableItem[] items)
        {
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Items = items;
        }
    }

    public enum ListState
    {
        Exited      = 0,
        InFocus     = 1,
        OutFocus    = 2,
    }

    /// <summary>
    /// 
    /// </summary>
	public class NavigationList : MonoBehaviour, INavigationElement
    {
        public ListName listName;

        public ListState State { get; protected set; }

        /// <summary>
        /// 数据更新次数
        /// </summary>
        public int UpateTime { get; protected set; }

        private bool isValid = true;
        /// <summary>
        /// 数据是否有效
        /// </summary>
        public bool IsValid 
        {
            get
            {
                return isValid;
            }
            set 
            { 
                isValid = value; 
            }
        }
        
        public virtual bool InFocus(params int[] indexs)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool OutFocus()
        {
            throw new System.NotImplementedException();
        }
        public virtual bool Refocus()
        {
            throw new System.NotImplementedException();
        }
        public virtual void Exit()
        {
            throw new NotImplementedException();
        }

        public virtual bool Move(Vector2 dir)
        {
            return false;
        }

        public virtual bool Select(params int[] indexs)
        {
            return false;
        }

        public virtual void UpdateTotalCount(int totalCount)
        {

        }
    }
}

