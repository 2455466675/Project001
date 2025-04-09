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
        
        public int ItemCount { get; private set; }

        public int TotalCount { get; private set; }

        public NavigationItem[] Items { get; private set; }
        public ListChangedEventArgs(int minIndex, int maxIndex, int itemCount, int totalCount, NavigationItem[] items)
        {
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            ItemCount = itemCount;
            TotalCount = totalCount;
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
        protected int minIndex;
        protected int maxIndex;
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

        [SerializeField]
        [Range(0.2f, 1f)]
        private float pressTime = 0.3f; //长按时间
        private float pressTimer;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float intervalTime = 0.15f; //长按后每次更新间隔
        private float intervalTimer;

        private bool isPress;
        private bool CanMove => pressTimer <= 0f && intervalTimer <= 0f;

        private void OnDestroy()
        {
            Clear();
        }

        private void FixedUpdate()
        {
            if (!isPress) 
            {
                return;
            }

            if (pressTimer > 0f)
            {
                pressTimer -= Time.fixedDeltaTime;
            }

            if (intervalTimer > 0f)
            {
                intervalTimer -= Time.fixedDeltaTime;
            }            
        }

        public virtual void Init() 
        {
        }

        public virtual void Clear() 
        {        
        }

        public void Move(float h, float v)
        {
            if (h == 0f && v == 0f) 
            {
                isPress = false;          
                return;
            }
            
            if (!isPress) 
            {
                MoveInner(h, v);
                isPress = true;
                pressTimer = pressTime;
                intervalTimer = 0f;
            }
            else
            {
                if (!CanMove)
                {
                    return;
                }
                MoveInner(h, v);
                intervalTimer = intervalTime;
            }            
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
            isPress = false;
            OutFocusCurrent();
        }

        public void Exit()
        {
            state = ListState.Closed;
            DeselectCurrent();
            current = null;
            isPress = false;
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

        private void MoveInner(float h, float v)
        {
            MoveCurrent(h, v);
            OnMove(h, v);
        }
    }
}
