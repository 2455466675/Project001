using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class LoopNavigationGroup : NavigationGroup
    {
        /// <summary>
        /// 当最大索引和最小索引发生变化时
        /// </summary>
        public event Action<IndexChangedEventArgs> OnIndexChangedEvent;
        /// <summary>
        /// 当前选择发生变化时
        /// </summary>
        public event Action<SelectChangedEventArgs> OnSelectChangedEvent;

        private Dictionary<int, NavigationItem> items;

        private int totalCount;
        private int itemCount;

        public void Init() 
        {
            isInit = true;
            totalCount = -1;
            minIndex = -1;
            maxIndex = -1;
            pointer = -1;
            state = GroupState.Exited;

            CreateItems();            
        }

        public override void OnExit()
        {
            base.OnExit();
            pointer = -1;
        }

        public override bool OnInFocus(bool isRefocus,params int[] indexs)
        {
            if (isRefocus) 
            {
                if (Select(pointer))
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
            float y = dir.y;
            int index;
            if (y > 0)
            {
                index = pointer - 1;
            }
            else if (y < 0)
            {
                index = pointer + 1;
            }
            else
            {
                return;
            }

            Select(index);
        }

        public override void OnSubmit()
        {
            base.OnSubmit();
        }

        public override void UpdateElementCount(int count)
        {
            if (!isInit) 
            {
                MLog.Error("列表尚未初始化");
                return;
            }

            if (count < 0)
            {
                return;
            }

            if (count == 0)
            {
                minIndex = 0;
                maxIndex = 0;
                pointer = 0;
                totalCount = 0;

                foreach (var item in items)
                {
                    item.Value.SetActive(false);
                }


                //退出
                return;
            }

            if (totalCount == count)
            {
                OnIndexChanged();
                return;
            }

            totalCount = count;

            int oldPointer = pointer;

            if (pointer == 0)
            {
                minIndex = 0;
                maxIndex = Mathf.Min(totalCount - 1, itemCount - 1);
            }
            else
            {
                int i = pointer - minIndex;

                maxIndex = Mathf.Min(totalCount - 1, Mathf.Max(maxIndex, itemCount - 1));
                minIndex = Mathf.Max(maxIndex - (itemCount - 1), 0);
                pointer = Mathf.Clamp(minIndex + i, minIndex, maxIndex);
            }

            OnIndexChanged();

            if (state == GroupState.InFocused)
            {
                OnSelectChanged(oldPointer != pointer);
            }
        }

        public override bool Select(params int[] indexs)
        {
            if (!isInit)
            {
                return false;
            }
            if (indexs == null || indexs.Length <= 0)
            {
                return false;
            }

            int index = indexs[0];

            bool isSuccess = false;

            if (index < 0 || index >= totalCount)
            {
                isSuccess = false;
            }
            else if (index >= minIndex && index <= maxIndex)
            {
                pointer = index;
                isSuccess = true;
            }
            else if (index < minIndex)
            {
                int i = minIndex - index;

                minIndex -= i;
                maxIndex -= i;
                pointer = minIndex;

                isSuccess = true;
                OnIndexChanged();
            }
            else if (index > maxIndex)
            {
                int i = index - maxIndex;

                minIndex += i;
                maxIndex += i;
                pointer = maxIndex;

                isSuccess = true;
                OnIndexChanged();
            }

            OnSelectChanged(isSuccess);
            return isSuccess;
        }

        private void OnIndexChanged()
        {
            int length = maxIndex - minIndex + 1;
            NavigationItem[] lts = new NavigationItem[length];

            foreach (var item in items)
            {
                item.Value.SetActive(item.Key < length);

                if (item.Key >= length)
                {
                    continue;
                }
                
                lts[item.Key] = item.Value;
            }

            OnIndexChangedEvent?.Invoke(new IndexChangedEventArgs(minIndex, maxIndex, lts));
        }

        private void OnSelectChanged(bool isSuccess)
        {
            NavigationItem[] selectedItems = new NavigationItem[] { items[pointer - minIndex] };
            OnSelectChangedEvent?.Invoke(new SelectChangedEventArgs(isSuccess, new int[] { pointer }, selectedItems));

            if (isSuccess) 
            {
                SelectChanged(selectedItems);
            }
        }

        private void CreateItems()
        {
            if (item == null)
            {
                MLog.Error("item is null");
                return;
            }

            item.SetActive(false);

            float vh = viewport.rect.size.y;
            RectTransform tf = item.GetComponent<RectTransform>();
            tf.anchorMin = new Vector2(0.5f, 1);
            tf.anchorMax = new Vector2(0.5f, 1);
            tf.pivot = new Vector2(0.5f, 0.5f);

            if (!content.TryGetComponent<VerticalLayoutGroup>(out var layoutGroup))
            {
                MLog.Warn("content没有LayoutGroup");
                return;
            }

            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);

            float topPadding = layoutGroup.padding.top;
            float bottomPadding = layoutGroup.padding.bottom;
            float spacing = layoutGroup.spacing;

            float h = tf.rect.size.y;
            int c = Mathf.FloorToInt((vh - topPadding - bottomPadding) / (h + spacing / 2));
            items = new Dictionary<int, NavigationItem>(c);
            for (int i = 0; i < c; i++)
            {
                NavigationItem lt = GoHelper.Instantiate<NavigationItem>(item, content);
                lt.SetActive(false);
                lt.SetIndex(i);
                items[i] = lt;
            }
            itemCount = items.Count;
        }
    }
}
