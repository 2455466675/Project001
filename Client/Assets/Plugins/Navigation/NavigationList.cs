using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation 
{
    public enum ListState
    {
        Closed = 0,
        InFocused = 1,
        OutFocused = 2,
    }

    public enum ListType
    {
        Vertical = 0,
        Horizontal = 1,
        Grid = 2,
    }


    public abstract class NavigationList : MonoBehaviour
    {
        [SerializeField]
        protected ListType m_ListType;
        protected ListState state;

        protected int minIndex;
        protected int maxIndex;
        protected int pointer;
        protected bool isInit;

        private NavigationItem[] current;

        #region Event

        public event Action OnListInFocus;
        public event Action OnListOutFocus;
        public event Action OnListExit;

        public event Action<NavigationItem> OnSelectedItem;
        public event Action<NavigationItem> OnDeselectedItem;
        public event Action<NavigationItem> OnSubmitItem;    
        public event Action<float, float, NavigationItem> OnMoveItem;

        public event Action<NavigationItem> OnItemBindData;
        public event Action<NavigationItem> OnItemUnbindData;

        #endregion

        #region

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

        #endregion

        #region

        public bool InFocus(bool isRefocus, int[] indexs = null) 
        {
            if (indexs == null || indexs.Length == 0)
            {
                indexs = new int[] { 0 };
            }
            bool r = OnInFocus(isRefocus, indexs);
            if (r)
            {
                OnListInFocus?.Invoke();
            }
            return r;
        }

        public void OutFocus() 
        {
            state = ListState.OutFocused;
            isPress = false;
            OutFocusCurrent();
            OnListOutFocus?.Invoke();
        }

        public void Exit() 
        {
            state = ListState.Closed;
            DeselectCurrent();
            current = null;
            isPress = false;
            OnExit();
            OnListExit?.Invoke();
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

        #endregion

        #region

        public abstract void Init();
        public abstract void UpdateDataCount(int count);      

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

        protected void SetItemDataIndex(NavigationItem item, int index) 
        {
            if (index < 0)
            {
                UnbindData(item);
            }
            else
            {
                BindData(item, index);
            }
        }

        #endregion

        #region

        private void MoveInner(float h, float v)
        {
            MoveCurrent(h, v);
            OnMove(h, v);
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

                    OnMoveItem?.Invoke(h, v, item);
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
                    OnSubmitItem?.Invoke(item);
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
                    OnSelectedItem?.Invoke(item);
                }
            }
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
                    OnDeselectedItem?.Invoke(item);
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

        private void BindData(NavigationItem item, int index)
        {
            if (item.IndexOfData >= 0)
            {
                UnbindData(item);
            }

            item.SetDataIndex(index);
            OnItemBindData?.Invoke(item);
        }

        private void UnbindData(NavigationItem item)
        {
            if (item.IndexOfData < 0)
            {
                return;
            }

            OnItemUnbindData?.Invoke(item);
            item.SetDataIndex(-1);
        }

        #endregion
    }
}