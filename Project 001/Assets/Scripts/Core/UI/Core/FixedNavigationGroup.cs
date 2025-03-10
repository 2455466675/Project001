using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class FixedNavigationGroup : NavigationGroup
    {
        [SerializeField]
        private bool isLoop;

        private List<NavigationItem> items;
        private Stack<NavigationItem> pool;

        private int TotalCount => items.Count;

        private int[] index;
        private bool IsMultiple => index != null && index.Length > 1;

        /// <summary>
        /// 当前选择发生变化时
        /// </summary>
        public event Action<SelectChangedEventArgs> OnSelectChangedEvent;

        public void Init()
        {
            if (item != null)
            {
                item.SetActive(false);
                (item.transform as RectTransform).pivot = new Vector2(0.5f, 0.5f);
            }
            else
            {
                MLog.Error("item is null");
                return;
            }

            state = GroupState.Exited;
            items = new List<NavigationItem>();
            pool = new Stack<NavigationItem>();
            isInit = true;
        }

        public NavigationItem GetItem(int index)
        {
            if (items == null || items.Count == 0)
            {
                return null;
            }
            if (index < 0 || index >= items.Count)
            {
                return null;
            }
            return items[index];
        }

        public override void UpdateElementCount(int count)
        {
            if (!isInit)
            {
                MLog.Error("列表尚未初始化");
                return;
            }

            if (TotalCount < count)
            {
                int c = count - TotalCount;
                for (int i = 0; i < c; i++)
                {
                    NavigationItem it;
                    if (pool.Count > 0)
                    {
                        it = pool.Pop();
                        it.SetActive(true);
                    }
                    else
                    {
                        it = GoHelper.Instantiate<NavigationItem>(item, content);
                        it.SetActive(true);
                    }
                    items.Add(it);
                    it.SetIndex(items.Count - 1);
                }
            }
            else if (TotalCount > count)
            {
                int c = TotalCount - count;
                for (int i = 0; i < c; i++)
                {
                    int index = items.Count - 1 - i;
                    NavigationItem it = items[index];
                    it.SetActive(false);
                    pool.Push(it);
                    items.RemoveAt(index);
                }

                if (pointer >= items.Count && count > 0)
                {
                    pointer = items.Count - 1;
                    Select(pointer);
                }
            }

            if (count == 0)
            {

            }
        }

        public override void OnExit()
        {
            base.OnExit();
            index = null;
        }

        public override bool OnInFocus(bool isRefocus, params int[] indexs)
        {
            if (isRefocus) 
            {
                if (Select(index))
                {
                    state = GroupState.InFocused;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (Select(indexs))
                {
                    state = GroupState.InFocused;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override void OnOutFocus()
        {
            base.OnOutFocus();                
        }

        public override void OnMove(Vector2 dir)
        {
            if (!isInit)
            {
                return;
            }

            if (IsMultiple)
            {
                MLog.Log("有多个选中元素时不允许此操作,元素数量:", this.index.Length);
                return;
            }

            float y = dir.y;
            int index;
            if (y > 0)
            {
                index = pointer - 1;
                if (index < 0 && isLoop)
                {
                    index = TotalCount - 1;
                }
            }
            else if (y < 0)
            {
                index = pointer + 1;
                if (index >= TotalCount && isLoop)
                {
                    index = 0;
                }
            }
            else
            {
                return;
            }

            Select(index);
        }

        public override bool Select(params int[] indexs)
        {
            if (!isInit)
            {
                return false;
            }
            if (items == null || items.Count <= 0)
            {
                return false;
            }
            if (indexs == null || indexs.Length <= 0)
            {
                return false;
            }

            bool isSuccess = true;
            for (int i = 0; i < indexs.Length; i++)
            {
                if (indexs[i] < 0 || indexs[i] >= items.Count)
                {
                    isSuccess = false;
                    break;
                }
            }

            NavigationItem[] argItems = null;
            if (isSuccess)
            {
                argItems = new NavigationItem[indexs.Length];
                for (int i = 0; i < indexs.Length; i++)
                {
                    int j = indexs[i];
                    argItems[i] = items[j];
                }

                argItems = argItems.Where(item => item.IsValid).ToArray();
                index = argItems?.Select(item => item.Index).ToArray();

                isSuccess = argItems != null && argItems.Length > 0;
                pointer = argItems != null ? items.IndexOf(argItems[0]) : 0;
            }

            OnSelectChangedEvent?.Invoke(new SelectChangedEventArgs(isSuccess, indexs, argItems));

            if (isSuccess) 
            {
                SelectChanged(argItems);
            }

            return isSuccess;
        }
    }
}
