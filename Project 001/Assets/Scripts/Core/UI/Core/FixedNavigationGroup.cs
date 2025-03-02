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

        private float topPadding;
        private float bottomPadding;

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

            if (viewport == null)
            {
                MLog.Error("FixedNavigationList没有viewport");
                return;
            }

            if (!content.TryGetComponent<LayoutGroup>(out var layoutGroup))
            {
                MLog.Warn("FixedNavigationList初始化，content没有LayoutGroup");
                return;
            }

            topPadding = layoutGroup.padding.top;
            bottomPadding = layoutGroup.padding.bottom;

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

        public override void OnInFocus(bool isRefocus, params int[] indexs)
        {
            if (isRefocus) 
            {
                if (Select(index))
                {
                    state = GroupState.InFocused;
                }
            }
            else
            {
                if (Select(indexs))
                {
                    state = GroupState.InFocused;
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

            if (argItems != null && argItems.Length == 1)
            {
                Vector3[] corners = new Vector3[4];
                viewport.GetWorldCorners(corners);

                RectTransform rt = argItems[0].transform as RectTransform;

                Vector3 leftBottom = rt.parent.InverseTransformPoint(corners[0]);
                Vector3 rightTop = rt.parent.InverseTransformPoint(corners[2]);

                float h = rt.rect.height / 2;
                float y = rt.localPosition.y;

                if (leftBottom.y > y - h)
                {
                    float c = leftBottom.y - (y - h) + bottomPadding;
                    content.localPosition = new Vector3(content.localPosition.x, content.localPosition.y + c, 0);
                }
                else if (rightTop.y < y + h)
                {
                    float c = (y + h) - rightTop.y + topPadding;
                    content.localPosition = new Vector3(content.localPosition.x, content.localPosition.y - c, 0);
                }
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
