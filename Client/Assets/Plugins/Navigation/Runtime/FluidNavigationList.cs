using System;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 流式导航列表：Item 数量固定、通过复用 Item 承载大量数据的虚拟滚动列表。
    /// 支持垂直、水平、网格三种布局。
    /// </summary>
    public class FluidNavigationList : NavigationList
    {
        #region Alignment
        private interface IAlignment
        {
            int MovePointer(float h, float v, int pointer, int max);
        }

        [Serializable]
        private class Vertical : IAlignment
        {
            public enum Alignment
            {
                TopToBottom,
                BottomToTop,
            }

            public float horizontalOffset;
            public float verticalOffset;
            public float spacing;
            public bool isLoop;
            public Alignment alignment;

            public int MovePointer(float h, float v, int pointer, int max)
            {
                int index = -1;
                bool topToBottom = alignment == Alignment.TopToBottom;
                if (v > 0)
                {
                    if (topToBottom)
                    {
                        index = pointer - 1;
                        if (index < 0 && isLoop)
                        {
                            index = max;
                        }
                    }
                    else
                    {
                        index = pointer + 1;
                        if (index > max && isLoop)
                        {
                            index = 0;
                        }
                    }
                }
                else if (v < 0)
                {
                    if (topToBottom)
                    {
                        index = pointer + 1;
                        if (index > max && isLoop)
                        {
                            index = 0;
                        }
                    }
                    else
                    {
                        index = pointer - 1;
                        if (index < 0 && isLoop)
                        {
                            index = max;
                        }
                    }
                }
                return index;
            }
        }

        [Serializable]
        private class Horizontal : IAlignment
        {
            public enum Alignment
            {
                LeftToRight,
                RightToLeft,
            }

            public float horizontalOffset;
            public float verticalOffset;
            public float spacing;
            public bool isLoop;
            public Alignment alignment;

            public int MovePointer(float h, float v, int pointer, int max)
            {
                int index = -1;
                bool leftToRight = alignment == Alignment.LeftToRight;
                if (h > 0)
                {
                    if (leftToRight)
                    {
                        index = pointer + 1;
                        if (index > max && isLoop)
                        {
                            index = 0;
                        }
                    }
                    else
                    {
                        index = pointer - 1;
                        if (index < 0 && isLoop)
                        {
                            index = max;
                        }
                    }
                }
                else if (h < 0)
                {
                    if (leftToRight)
                    {
                        index = pointer - 1;
                        if (index < 0 && isLoop)
                        {
                            index = max;
                        }
                    }
                    else
                    {
                        index = pointer + 1;
                        if (index > max && isLoop)
                        {
                            index = 0;
                        }
                    }
                }
                return index;
            }
        }

        [Serializable]
        private class Grid : IAlignment
        {
            public enum Alignment
            {
                UpperLeft,
                UpperRight,
                LowerLeft,
                LowerRight,
            }

            public float horizontalOffset;
            public float verticalOffset;
            public float xSpacing;
            public float ySpacing;
            public Alignment alignment;

            public int rowCount;
            public int columnCount;

            public int MovePointer(float h, float v, int pointer, int max)
            {
                int index = -1;
                bool upperLeft = alignment == Alignment.UpperLeft;
                bool upperRight = alignment == Alignment.UpperRight;
                bool lowerLeft = alignment == Alignment.LowerLeft;
                bool lowerRight = alignment == Alignment.LowerRight;
                if (v > 0)
                {
                    if (upperLeft || upperRight)
                    {
                        index = pointer - columnCount;
                    }

                    if (lowerLeft || lowerRight)
                    {
                        index = pointer + columnCount;
                    }
                }
                else if (v < 0)
                {
                    if (upperLeft || upperRight)
                    {
                        index = pointer + columnCount;
                    }

                    if (lowerLeft || lowerRight)
                    {
                        index = pointer - columnCount;
                    }
                }

                if (h > 0)
                {
                    if (upperLeft || lowerLeft)
                    {
                        if ((pointer + 1) % columnCount > 0)
                        {
                            index = pointer + 1;
                        }
                    }

                    if (upperRight || lowerRight)
                    {
                        if ((pointer) % columnCount > 0)
                        {
                            index = pointer - 1;
                        }
                    }
                }
                else if (h < 0)
                {
                    if (upperLeft || lowerLeft)
                    {
                        if ((pointer) % columnCount > 0)
                        {
                            index = pointer - 1;
                        }
                    }

                    if (upperRight || lowerRight)
                    {
                        if ((pointer + 1) % columnCount > 0)
                        {
                            index = pointer + 1;
                        }
                    }
                }
                return index;
            }
        }
        #endregion

        [SerializeField]
        [ShowIfEnum("m_ListType", ListType.Vertical)]
        private Vertical vertical;

        [SerializeField]
        [ShowIfEnum("m_ListType", ListType.Horizontal)]
        private Horizontal horizontal;

        [SerializeField]
        [ShowIfEnum("m_ListType", ListType.Grid)]
        private Grid grid;

        [SerializeField]
        private NavigationItem item;
        [SerializeField]
        private RectTransform viewport;
        [SerializeField]
        private RectTransform content;

        private bool Isvertical => m_ListType == ListType.Vertical;
        private bool IsHorizontal => m_ListType == ListType.Horizontal;
        private bool IsGrid => m_ListType == ListType.Grid;

        /// <summary>
        /// 数据数量
        /// </summary>
        private int dataCount;
        /// <summary>
        /// 子物体数量
        /// </summary>
        private int itemCount;
        private Dictionary<int, NavigationItem> items;

        public int ItemCount => itemCount;
        public int DataCount => dataCount;

        public override void Init()
        {
            if (isInit)
            {
                Debug.LogWarning("repeat init");
                return;
            }

            if (item == null)
            {
                Debug.LogError("item is null");
                return;
            }

            if (viewport == null)
            {
                Debug.LogError("viewport is null");
                return;
            }

            if (content == null)
            {
                Debug.LogError("content is null");
                return;
            }

            //Grid 布局的窗口对齐/移动运算依赖行列数做除法与取模，必须均为正。
            if (IsGrid && (grid.rowCount <= 0 || grid.columnCount <= 0))
            {
                Debug.LogError("Grid 布局下 rowCount 与 columnCount 必须均大于 0。");
                return;
            }

            isInit = true;
            dataCount = -1;
            minIndex = -1;
            maxIndex = -1;
            pointer = -1;
            state = ListState.Closed;

            if (Isvertical)
            {
                CreateItemsByVertical();
            }
            if (IsHorizontal)
            {
                CreateItemsByHorizontal();
            }
            if (IsGrid)
            {
                CreateItemsByGrid();
            }
        }

        public override void UpdateDataCount(int count)
        {
            if (!isInit)
            {
                Debug.LogError("do not init");
                return;
            }

            if (count < 0)
            {
                return;
            }

            if (count == 0)
            {
                dataCount = 0;

                //隐藏并解绑所有 Item，清空可视内容。
                if (items != null)
                {
                    foreach (var kv in items)
                    {
                        SetItemDataIndex(kv.Value, -1);
                        kv.Value.SetActive(false);
                    }
                }

                //列表已空：若正处于聚焦态，释放焦点并清空选中，避免指针指向不存在的数据；
                //否则仅重置窗口状态。
                if (state == ListState.InFocused)
                {
                    Exit();
                }
                else
                {
                    minIndex = -1;
                    maxIndex = -1;
                    pointer = -1;
                }
                return;
            }

            if (dataCount == count)
            {
                OnListChanged();
                return;
            }

            dataCount = count;

            if (pointer == 0)
            {
                minIndex = 0;
                maxIndex = Mathf.Min(dataCount - 1, itemCount - 1);
                OnListChanged();
            }
            else
            {
                int oldPointer = pointer;
                if (IsGrid)
                {
                    int oldMin = minIndex;

                    maxIndex = Mathf.Min(dataCount - 1, Mathf.Max(maxIndex, itemCount - 1));
                    if ((maxIndex + 1) % grid.columnCount == 0)
                    {
                        minIndex = Mathf.Max(maxIndex - (grid.rowCount * grid.columnCount) + 1, 0);
                    }
                    else
                    {
                        minIndex = Mathf.Max(maxIndex - ((grid.rowCount - 1) * grid.columnCount + (maxIndex + 1) % grid.columnCount) + 1, 0);
                    }

                    pointer = Mathf.Clamp(pointer, minIndex, maxIndex);

                    OnListChanged();
                    if (state == ListState.InFocused && (oldPointer != pointer || Mathf.Abs(oldMin - minIndex) >= grid.columnCount))
                    {
                        OnSelectChanged();
                    }
                }
                else
                {
                    int i = pointer - minIndex;

                    maxIndex = Mathf.Min(dataCount - 1, Mathf.Max(maxIndex, itemCount - 1));
                    minIndex = Mathf.Max(maxIndex - (itemCount - 1), 0);
                    pointer = Mathf.Clamp(minIndex + i, minIndex, maxIndex);

                    OnListChanged();
                    if (state == ListState.InFocused && oldPointer != pointer)
                    {
                        OnSelectChanged();
                    }
                }
            }
        }

        protected override void OnExit()
        {
            pointer = -1;
            minIndex = -1;
            maxIndex = -1;
            dataCount = 0;
        }

        protected override bool OnInFocus(bool isRefocus, int[] indexs)
        {
            int index = isRefocus ? pointer : indexs[0];
            if (Select(index))
            {
                OnSelectChanged();
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
            if (!isInit)
            {
                return;
            }

            int index = -1;
            if (Isvertical)
            {
                index = vertical.MovePointer(h, v, pointer, dataCount - 1);
            }

            if (IsHorizontal)
            {
                index = horizontal.MovePointer(h, v, pointer, dataCount - 1);
            }

            if (IsGrid)
            {
                index = grid.MovePointer(h, v, pointer, dataCount - 1);
            }

            if (index < 0)
            {
                return;
            }

            bool success = Select(index);
            if (success)
            {
                OnSelectChanged();
            }
        }

        private bool Select(int index)
        {
            if (!isInit)
            {
                return false;
            }

            if (index < 0 || index >= dataCount)
            {
                return false;
            }

            if (index >= minIndex && index <= maxIndex)
            {
                pointer = index;
                return true;
            }

            // index 落在当前窗口之外，需要重新对齐窗口，使其可见。
            if (IsGrid)
            {
                // Grid 以“行”为单位滑动。按目标索引所在行重新计算顶部行，
                // 保证无论跳跃多少行，pointer 都落在 rowCount 行的可见窗口内，
                // 从而 pointer - minIndex 始终位于 [0, itemCount) 区间。
                int cc = grid.columnCount;
                int rc = grid.rowCount;
                int topRow = minIndex / cc;
                int indexRow = index / cc;

                if (indexRow < topRow)
                {
                    topRow = indexRow;
                }
                else if (indexRow > topRow + rc - 1)
                {
                    topRow = indexRow - (rc - 1);
                }

                minIndex = topRow * cc;
                maxIndex = Mathf.Min(dataCount - 1, minIndex + rc * cc - 1);
            }
            else
            {
                // 线性列表按跳跃距离精确平移整个窗口，使 pointer 落到窗口边缘。
                if (index < minIndex)
                {
                    int i = minIndex - index;
                    minIndex -= i;
                    maxIndex -= i;
                }
                else // index > maxIndex
                {
                    int i = index - maxIndex;
                    minIndex += i;
                    maxIndex += i;
                }
            }

            pointer = index;
            OnListChanged();
            return true;
        }

        private void OnListChanged()
        {
            int length = (maxIndex < 0 || minIndex < 0) ? 0 : maxIndex - minIndex + 1;

            foreach (var kv in items)
            {
                int index = kv.Key;
                NavigationItem item = kv.Value;
                if (index < length)
                {
                    item.SetActive(true);
                    SetItemDataIndex(item, index + minIndex);
                }
                else
                {
                    SetItemDataIndex(item, -1);
                    item.SetActive(false);
                }
            }            
        }

        private void OnSelectChanged()
        {
            int key = pointer - minIndex;
            if (items == null || !items.TryGetValue(key, out NavigationItem selected) || selected == null)
            {
                Debug.LogError($"[FluidNavigationList] 选中项无对应可见Item。pointer:{pointer} minIndex:{minIndex} key:{key} itemCount:{itemCount}");
                return;
            }

            SelectChanged(new NavigationItem[] { selected });
        }

        #region

        private void CreateItemsByVertical()
        {
            item.SetActive(false);
            items = new Dictionary<int, NavigationItem>();

            bool topToBottom = vertical.alignment == Vertical.Alignment.TopToBottom;
            Vector2 pivot = new Vector2(0.5f, topToBottom ? 1f : 0f);
            RectTransform tf = item.GetComponent<RectTransform>();
            tf.anchorMin = new Vector2(0.5f, 0.5f);
            tf.anchorMax = new Vector2(0.5f, 0.5f);
            tf.pivot = pivot;

            content.anchorMin = new Vector2(0f, 0f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = pivot;
            content.sizeDelta = Vector2.zero;

            float verticalOffset = vertical.verticalOffset;
            float horizontalOffset = vertical.horizontalOffset;

            float spacing = vertical.spacing;

            float vh = viewport.rect.size.y;
            float h = tf.rect.size.y;
            float denom = h + spacing / 2f;
            int c = denom > 0f ? Mathf.FloorToInt((vh - verticalOffset) / denom) : 0;
            c = Mathf.Max(c, 0);   //防止分母非正或视口过小时得到非法数量

            for (int i = 0; i < c; i++)
            {
                NavigationItem lt = Instantiate<NavigationItem>(item, content);

                float x = 0f + horizontalOffset;
                float y = (h * i + verticalOffset + spacing * i) * (topToBottom ? -1f : 1f);

                lt.transform.localPosition = new Vector2(x, y);
                lt.SetListIndex(i);
                lt.SetDataIndex(-1);
                lt.SetActive(false);
                items[i] = lt;
            }
            itemCount = items.Count;
        }

        private void CreateItemsByHorizontal()
        {
            item.SetActive(false);
            items = new Dictionary<int, NavigationItem>();

            bool leftToRight = horizontal.alignment == Horizontal.Alignment.LeftToRight;
            Vector2 pivot = new Vector2(leftToRight ? 0f : 1f, 0.5f);
            RectTransform tf = item.GetComponent<RectTransform>();
            tf.anchorMin = new Vector2(0.5f, 0.5f);
            tf.anchorMax = new Vector2(0.5f, 0.5f);
            tf.pivot = pivot;

            content.anchorMin = new Vector2(0f, 0f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = pivot;
            content.sizeDelta = Vector2.zero;

            float verticalOffset = horizontal.verticalOffset;
            float horizontalOffset = horizontal.horizontalOffset;
            float spacing = horizontal.spacing;

            float vw = viewport.rect.size.x;
            float w = tf.rect.size.x;
            float denom = w + spacing / 2f;
            int c = denom > 0f ? Mathf.FloorToInt((vw - horizontalOffset) / denom) : 0;
            c = Mathf.Max(c, 0);   //防止分母非正或视口过小时得到非法数量

            for (int i = 0; i < c; i++)
            {
                NavigationItem lt = Instantiate<NavigationItem>(item, content);

                float x = (w * i + horizontalOffset + spacing * i) * (leftToRight ? 1f : -1f);
                float y = 0f + verticalOffset;

                lt.transform.localPosition = new Vector2(x, y);
                lt.SetListIndex(i);
                lt.SetDataIndex(-1);
                lt.SetActive(false);
                items[i] = lt;
            }
            itemCount = items.Count;
        }

        private void CreateItemsByGrid()
        {
            item.SetActive(false);
            items = new Dictionary<int, NavigationItem>();

            bool upperLeft = grid.alignment == Grid.Alignment.UpperLeft;
            bool upperRight = grid.alignment == Grid.Alignment.UpperRight;
            bool lowerLeft = grid.alignment == Grid.Alignment.LowerLeft;
            bool lowerRight = grid.alignment == Grid.Alignment.LowerRight;

            Vector2 pivot = new Vector2();
            if (upperLeft)
            {
                pivot.x = 0f;
                pivot.y = 1f;
            }
            if (upperRight)
            {
                pivot.x = 1f;
                pivot.y = 1f;
            }
            if (lowerLeft)
            {
                pivot.x = 0f;
                pivot.y = 0f;
            }
            if (lowerRight)
            {
                pivot.x = 1f;
                pivot.y = 0f;
            }

            RectTransform tf = item.GetComponent<RectTransform>();
            tf.anchorMin = new Vector2(0.5f, 0.5f);
            tf.anchorMax = new Vector2(0.5f, 0.5f);
            tf.pivot = pivot;

            content.anchorMin = new Vector2(0f, 0f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = pivot;
            content.sizeDelta = Vector2.zero;

            float verticalOffset = grid.verticalOffset;
            float horizontalOffset = grid.horizontalOffset;
            float xSpacing = grid.xSpacing;
            float ySpacing = grid.ySpacing;

            float w = tf.rect.size.x;
            float h = tf.rect.size.y;

            int rc = grid.rowCount;
            int cc = grid.columnCount;

            for (int i = 0; i < rc; i++)
            {
                for (int j = 0; j < cc; j++)
                {
                    NavigationItem lt = Instantiate<NavigationItem>(item, content);

                    float x = w * j + horizontalOffset + xSpacing * j;
                    float y = h * i + verticalOffset + ySpacing * i;

                    if (upperLeft)
                    {
                        y *= -1f;
                    }

                    if (upperRight)
                    {
                        x *= -1f;
                        y *= -1f;
                    }

                    if (lowerRight)
                    {
                        x *= -1f;
                    }

                    int index = cc * i + j;
                    lt.transform.localPosition = new Vector2(x, y);
                    lt.SetListIndex(index);
                    lt.SetDataIndex(-1);
                    lt.SetActive(false);
                    items[index] = lt;
                }
            }

            itemCount = rc * cc;
        }

        #endregion
    }
}
