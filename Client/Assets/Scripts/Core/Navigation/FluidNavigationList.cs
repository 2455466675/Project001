using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class FluidNavigationList : NavigationList
    {
        /// <summary>
        /// 当列表发生变化时
        /// </summary>
        public event Action<ListChangedEventArgs> OnListChangedEvent;

        [SerializeField]
        protected RectTransform viewport;
        [SerializeField]
        protected RectTransform content;

        private int totalCount;
        private int itemCount;
        private Dictionary<int, NavigationItem> items;

        public override void Init()
        {
            isInit = true;
            totalCount = -1;
            minIndex = -1;
            maxIndex = -1;
            pointer = -1;
            state = ListState.Closed;

            CreateItems();
        }

        public void UpdateItemCount(int count)
        {
            if (!isInit)
            {
                Debug.LogError("列表尚未初始化");
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

                //TODO:退出
                return;
            }

            if (totalCount == count)
            {
                OnListChanged();
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

            OnListChanged();

            if (state == ListState.InFocused)
            {
                OnSelectChanged(oldPointer != pointer);
            }
        }

        protected override void OnExit()
        {
            pointer = -1;
        }

        protected override bool OnInFocus(bool isRefocus, int[] indexs)
        {
            int index = isRefocus ? pointer : indexs[0];
            if (Select(index))
            {
                state = ListState.InFocused;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnMove(float h, float v)
        {
            float y = v;
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

        private bool Select(int index)
        {
            if (!isInit)
            {
                return false;
            }

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
                OnListChanged();
            }
            else if (index > maxIndex)
            {
                int i = index - maxIndex;

                minIndex += i;
                maxIndex += i;
                pointer = maxIndex;

                isSuccess = true;
                OnListChanged();
            }

            OnSelectChanged(isSuccess);
            return isSuccess;
        }

        private void OnListChanged()
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

            OnListChangedEvent?.Invoke(new ListChangedEventArgs(minIndex, maxIndex, lts));
        }

        private void OnSelectChanged(bool isSuccess)
        {      
            if (isSuccess)
            {
                NavigationItem[] selectedItems = new NavigationItem[] { items[pointer - minIndex] };
                SelectChanged(selectedItems);
            }
        }

        private void CreateItems()
        {
            if (item == null)
            {
                Debug.LogError("item is null");
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
                Debug.LogError("content没有LayoutGroup");
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
                NavigationItem lt = Instantiate<NavigationItem>(item, content);
                lt.SetActive(false);
                lt.SetIndex(i);
                items[i] = lt;
            }
            itemCount = items.Count;
        }
    }
}
