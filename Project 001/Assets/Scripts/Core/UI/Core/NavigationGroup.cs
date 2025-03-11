using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public enum GroupState
    {
        Exited     = 0,
        InFocused  = 1,
        OutFocused = 2,
    }

    public enum GroupType
    {
        Vertical   = 0,
        Horizontal = 1,
        Grid       = 2,
    }

    public struct SelectChangedEventArgs
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
        public NavigationItem[] Items { get; private set; }
        public SelectChangedEventArgs(bool isSuccess, int[] index, NavigationItem[] items)
        {
            IsSuccesss = isSuccess;
            Index = index;
            Items = items;
        }
    }

    public struct IndexChangedEventArgs
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
        public NavigationItem[] Items { get; private set; }
        public IndexChangedEventArgs(int minIndex, int maxIndex, NavigationItem[] items)
        {
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Items = items;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroup : MonoBehaviour
    {
        private static Dictionary<UIDefine.Group_ID, NavigationGroup> groups;

        public static void AddNavigationGroup(NavigationGroup group) 
        {
            if (groups == null) 
            {
                groups = new Dictionary<UIDefine.Group_ID, NavigationGroup>();
            }

            if (group.groupID == UIDefine.Group_ID.Undefined) 
            {
                return;
            }

            if (groups.ContainsKey(group.groupID)) 
            {
                return;
            }
            groups.Add(group.groupID, group);
        }

        public static NavigationGroup GetNavigationGroup(UIDefine.Group_ID groupID) 
        {
            if(groups.ContainsKey(groupID)) 
            {
                return groups[groupID];
            }
            else
            {
                return null;
            }
        }

        public UIDefine.Group_ID groupID;

        [SerializeField]
        protected NavigationItem item;

        protected int minIndex;
        protected int maxIndex;
        protected int pointer;

        [SerializeField]
        protected RectTransform viewport;
        [SerializeField]
        protected RectTransform content;
        [SerializeField]
        private CanvasGroup canvasGroup;

        protected GroupState state;
        protected bool isInit;

        private NavigationItem[] current;

        private void Awake()
        {
            AddNavigationGroup(this);
        }

        /// <summary>
        /// 当退出时
        /// </summary>
        public virtual void OnExit() 
        {
            DeselectCurrent();
            current = null;
            state = GroupState.Exited;
        }

        /// <summary>
        /// 当聚焦时
        /// </summary>
        public virtual bool OnInFocus(bool isRefocus, params int[] indexs)
        {
            return false;
        }

        /// <summary>
        /// 当失焦时
        /// </summary>
        public virtual void OnOutFocus()
        {
            state = GroupState.OutFocused;
            OutFocusCurrent();
        }

        /// <summary>
        /// 移动指针指令
        /// </summary>
        /// <param name="dir"></param>
        public virtual void OnMove(Vector2 dir) 
        {        
        }

        /// <summary>
        /// 当确认按下时
        /// </summary>
        public virtual void OnSubmit() 
        {            
            SubmitCurrent();
        }

        public virtual bool Select(params int[] indexs)
        {
            return false;
        }

        public virtual void UpdateElementCount(int count) 
        {        
        }

        protected void SelectChanged(NavigationItem[] items) 
        {
            DeselectCurrent();
            SelectCurrent(items);
        }

        private void DeselectCurrent() 
        {
            if (current != null)
            {
                NavigationItem[] temp = new NavigationItem[current.Length];
                Array.Copy(current, temp, current.Length);

                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i].OnDeselect();
                }
            }
        }

        private void SelectCurrent(NavigationItem[] items)
        {
            current = items;

            if (current != null)
            {
                NavigationItem[] temp = new NavigationItem[current.Length];
                Array.Copy(current, temp, current.Length);

                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i].OnSelect();
                }
            }
        }

        private void OutFocusCurrent() 
        {
            if (current != null)
            {
                NavigationItem[] temp = new NavigationItem[current.Length];
                Array.Copy(current, temp, current.Length);

                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i].OutFocus();
                }
            }
        }

        private void SubmitCurrent()
        {
            if (current != null)
            {
                NavigationItem[] temp = new NavigationItem[current.Length];
                Array.Copy(current, temp, current.Length);

                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i].OnSubmit();
                }
            }
        }
    }
}
