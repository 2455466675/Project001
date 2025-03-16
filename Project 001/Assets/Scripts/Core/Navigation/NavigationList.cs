using System;
using UnityEngine;

namespace Navigation
{
    public enum ListState
    {
        Closed     = 0,
        InFocused  = 1,
        OutFocused = 2,
    }

    public enum ListType
    {
        Vertical   = 0,
        Horizontal = 1,
        Grid       = 2,
    }

    public enum ChildAlignment
    {
        UpperLeft,
        UpperRight,
        LowerLeft,
        LowerRight,
    }

    public struct ListChangedEventArgs
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
        public ListChangedEventArgs(int minIndex, int maxIndex, NavigationItem[] items)
        {
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Items = items;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NavigationList : MonoBehaviour
    {
        [SerializeField]
        protected NavigationItem item;
        [SerializeField]
        protected ListType listType;

        protected int minIndex;
        protected int maxIndex;
        protected int pointer;

        protected bool isInit;
        protected ListState state;

        private NavigationItem[] current;

        public virtual void Init() 
        {
        }

        public void Move(float h, float v)
        {
            OnMove(h, v);
        }

        public void Submit()
        {
            SubmitCurrent();
        }
        public void OutFocus()
        {
            state = ListState.OutFocused;
            OutFocusCurrent();
        }
        public bool Enter(bool isRefocus, int[] indexs = null)
        {
            return OnEnter(isRefocus, indexs);
        }
        public void Exit()
        {
            state = ListState.Closed;
            DeselectCurrent();
            current = null;
            OnExit();
        }

        protected virtual void OnMove(float h, float v) 
        {
        }

        protected virtual bool OnEnter(bool isRefocus, int[] indexs = null) 
        {
            return false;
        }

        protected virtual void OnExit() 
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
