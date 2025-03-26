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
    public class NavigationList : MonoBehaviour, INavigation
    {
        [SerializeField]
        protected ListType listType;

        //[SerializeField]
        protected int minIndex;
        //[SerializeField]
        protected int maxIndex;
        //[SerializeField]
        protected int pointer;

        protected bool isInit;
        protected ListState state;

        private NavigationItem[] current;

        public event Action OnListEmptyEvent;

        public event Action<NavigationItem> OnSelectedEvent;
        public event Action<NavigationItem> OnDeselectedEvent;
        public event Action<NavigationItem> OnSubmitEvent;
        public event Action<NavigationItem> OnClearItemEvent;
        public event Action<float, float, NavigationItem> OnMoveEvent;

        public virtual void Init() 
        {
        }

        public virtual void Clear() 
        {        
        }

        public void Move(float h, float v)
        {
            MoveCurrent(h, v);
            OnMove(h, v);
        }

        public void Submit()
        {
            SubmitCurrent();
        }

        public bool InFocus(bool isRefocus, int[] indexs = null)
        {
            if (indexs == null || indexs.Length == 0) 
            {
                indexs = new int[] { 0 };
            }
            return OnInFocus(isRefocus, indexs);
        }

        public void OutFocus()
        {
            state = ListState.OutFocused;
            OutFocusCurrent();
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

        protected virtual bool OnInFocus(bool isRefocus, int[] indexs = null) 
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

        protected void ClearItem(NavigationItem item) 
        {
            OnClearItemEvent?.Invoke(item);
            item.UnbindData();
        }

        protected void ListEmpty() 
        {
            OnListEmptyEvent?.Invoke();
        }

        private void DeselectCurrent()
        {
            if (current != null)
            {
                NavigationItem[] temp = new NavigationItem[current.Length];
                Array.Copy(current, temp, current.Length);

                for (int i = 0; i < temp.Length; i++)
                {
                    NavigationItem item = temp[i];
                    item.OnDeselect();
                    OnDeselectedEvent?.Invoke(item);
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
                    NavigationItem item = temp[i];
                    item.OnSelect();
                    OnSelectedEvent?.Invoke(item);
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
                    NavigationItem item = temp[i];
                    item.OnSubmit();
                    OnSubmitEvent?.Invoke(item);
                }
            }
        }

        private void MoveCurrent(float h, float v) 
        {
            if (current != null)
            {
                NavigationItem[] temp = new NavigationItem[current.Length];
                Array.Copy(current, temp, current.Length);

                for (int i = 0; i < temp.Length; i++)
                {
                    NavigationItem item = temp[i];

                    if (h > 0)
                    {
                        item.OnMoveRight();
                    }
                    if (h < 0)
                    {
                        item.OnMoveLeft();
                    }
                    if (v > 0)
                    {
                        item.OnMoveUp();
                    }
                    if (v < 0)
                    {
                        item.OnMoveDown();
                    }

                    OnMoveEvent?.Invoke(h, v, item);
                }
            }
        }
    }
}
