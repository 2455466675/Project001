using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Navigation
{
    /// <summary>
    /// 有多少个数据就产生多少个元素，且视口固定不变
    /// </summary>
	public class FixedNavigationList : NavigationList
	{
        [SerializeField]
        private GuidableItem item;

        public bool isLoop;

        private bool isInit;

        private List<GuidableItem> items;
        private Stack<GuidableItem> pool;

        private int TotalCount => items.Count;
        private int pointer;

        private int[] index;
        private bool IsMultiple => index != null && index.Length > 1;

        private float topPadding;
        private float bottomPadding;

        /// <summary>
        /// 当前选择发生变化时
        /// </summary>
        private event Action<SelectChangedEventArgs> OnSelectChangedEvent;

        public void Init(Action<SelectChangedEventArgs> selectHandler)
        {
            if (selectHandler == null)
            {
                isInit = false;
                return;
            }

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

            OnSelectChangedEvent += selectHandler;
            State = ListState.Exited;
            items = new List<GuidableItem>();
            pool = new Stack<GuidableItem>();
            isInit = true;
        }

        public override void UpdateTotalCount(int totalCount)
        {
            if (!isInit)
            {
                return;
            }

            if (TotalCount < totalCount)
            {
                int c = totalCount - TotalCount;
                for (int i = 0; i < c; i++)
                {
                    GuidableItem it;
                    if (pool.Count > 0)
                    {
                        it = pool.Pop();
                        it.SetActive(true);
                    }
                    else
                    {
                        it = GoHelper.Instantiate<GuidableItem>(item, content);
                        it.SetActive(true);
                    }                    
                    items.Add(it);
                }
            }
            else if (TotalCount > totalCount)
            {
                int c = TotalCount - totalCount;
                for (int i = 0; i < c; i++)
                {
                    int index = items.Count - 1 - i;
                    GuidableItem it = items[index];
                    it.SetDatum(null);
                    it.SetActive(false);
                    pool.Push(it);
                    items.RemoveAt(index);
                }

                if (pointer >= items.Count && totalCount > 0)
                {
                    pointer = items.Count - 1;
                    Select(pointer);
                }
            }

            UpateTime++;

            if (totalCount == 0)
            {
                BackInner();
            }
        }

        public override bool InFocus(params int[] indexs)
        {
            if (Select(indexs))
            {
                State = ListState.InFocus;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool OutFocus()
        {
            if (index == null || index.Length <= 0)
            {
                return false;
            }

            for (int i = 0; i < index.Length; i++)
            {
                GuidableItem item = items[index[i]];
                item.OutFocus();
            }
            State = ListState.OutFocus;
            SelectedItems = null;
            return true;
        }

        public override bool Refocus()
        {
            if (Select(index))
            {
                State = ListState.InFocus;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Exit()
        {
            index = null;
            State = ListState.Exited;
            SelectedItems = null;
        }

        public GuidableItem GetItem(int index)
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

        public override bool Move(Vector2 dir)
        {
            if (!isInit)
            {
                return false;
            }

            if (IsMultiple)
            {
                MLog.Log("有多个选中元素时不允许此操作,元素数量:", this.index.Length);
                return false;
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
                return false;
            }

            return Select(index);
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

            GuidableItem[] argItems = null;
            if (isSuccess)
            {
                argItems = new GuidableItem[indexs.Length];
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
                else if(rightTop.y < y + h)
                {
                    float c = (y + h) - rightTop.y + topPadding;
                    content.localPosition = new Vector3(content.localPosition.x, content.localPosition.y - c, 0);
                }
            }

            SelectedItems = argItems;
            OnSelectChangedEvent?.Invoke(new SelectChangedEventArgs(isSuccess, indexs, argItems));
            return isSuccess;
        }
    }
}

