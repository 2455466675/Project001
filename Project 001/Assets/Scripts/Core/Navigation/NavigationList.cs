using System;
using System.Collections;
using System.Collections.Generic;
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

        public ListState State {  get; protected set; }

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
        public virtual void Close()
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

