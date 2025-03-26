using Codice.Client.BaseCommands.BranchExplorer;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class FluidNavigationList : NavigationList
    {
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

            public float horizontalOffest;
            public float verticalOffest;
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

            public float horizontalOffest;
            public float verticalOffest;
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

            public float horizontalOffest;
            public float verticalOffest;
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

        [SerializeField]
        [ShowIf("listType", ListType.Vertical)]
        private Vertical vertical;

        [SerializeField]
        [ShowIf("listType", ListType.Horizontal)]
        private Horizontal horizontal;

        [SerializeField]
        [ShowIf("listType", ListType.Grid)]
        private Grid grid;
        [SerializeField]
        private NavigationItem item;
        [SerializeField]
        private RectTransform viewport;
        [SerializeField]
        private RectTransform content;

        private bool Isvertical => listType == ListType.Vertical;
        private bool IsHorizontal => listType == ListType.Horizontal;
        private bool IsGrid => listType == ListType.Grid;

        /// <summary>
        /// 当列表发生变化时
        /// </summary>
        public event Action<ListChangedEventArgs> OnListChangedEvent;
        /// <summary>
        /// 数据数量
        /// </summary>
        private int totalCount;
        /// <summary>
        /// 子物体数量
        /// </summary>
        private int itemCount;
        private Dictionary<int, NavigationItem> items;

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

            isInit = true;
            totalCount = -1;
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

        public override void Clear()
        {
            foreach (var kv in items)
            {
                NavigationItem item = kv.Value;
                if (item.IsBinded)
                {
                    ClearItem(item);
                }
                item.SetActive(false);
            }
        }

        public void UpdateItemCount(int count)
        {
            if (!isInit)
            {                
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

                Clear();
                ListEmpty();
                return;
            }

            if (totalCount == count)
            {
                OnListChanged();
                return;
            }

            totalCount = count;

            if (pointer == 0)
            {
                minIndex = 0;
                maxIndex = Mathf.Min(totalCount - 1, itemCount - 1);
                OnListChanged();
            }
            else
            {
                int oldPointer = pointer;
                if (IsGrid) 
                {
                    int oldMin = minIndex;

                    maxIndex = Mathf.Min(totalCount - 1, Mathf.Max(maxIndex, itemCount - 1));
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

                    maxIndex = Mathf.Min(totalCount - 1, Mathf.Max(maxIndex, itemCount - 1));
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
                index = vertical.MovePointer(h, v, pointer, totalCount - 1);
            }

            if (IsHorizontal) 
            {
                index = horizontal.MovePointer(h, v, pointer, totalCount - 1);
            }

            if (IsGrid) 
            {
                index = grid.MovePointer(h, v, pointer, totalCount - 1);
            }
            Debug.Log($"index : {index}");
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

            if (index < 0 || index >= totalCount)
            {
                return false;
            }

            if (index >= minIndex && index <= maxIndex)
            {
                pointer = index;                
                return true;
            }

            if (index < minIndex)
            {
                if (IsGrid) 
                {
                    minIndex -= grid.columnCount;
                    if((maxIndex + 1) % grid.columnCount == 0) 
                    {
                        maxIndex -= grid.columnCount;
                    }
                    else 
                    {
                        maxIndex -= (maxIndex + 1) % grid.columnCount;
                    }
                }
                else
                {
                    int i = minIndex - index;
                    minIndex -= i;
                    maxIndex -= i;
                }

                pointer = index;        
                OnListChanged();
                return true;
            }

            if (index > maxIndex)
            {
                if (IsGrid)
                {
                    minIndex += grid.columnCount;
                    maxIndex += grid.columnCount;
                    maxIndex = Mathf.Min(totalCount - 1, maxIndex);
                }
                else
                {
                    int i = index - maxIndex;
                    minIndex += i;
                    maxIndex += i;
                }

                pointer = index;                
                OnListChanged();
                return true;
            }
           
            return false;
        }

        private void OnListChanged()
        {
            int length = maxIndex - minIndex + 1;
            NavigationItem[] lts = new NavigationItem[length];

            foreach (var kv in items)
            {
                int index = kv.Key;
                NavigationItem item = kv.Value;
                if (index < length) 
                {
                    item.SetActive(true);
                    lts[index] = item;
                }
                else
                {
                    if (item.IsBinded) 
                    {
                        ClearItem(item);
                    }
                    item.SetActive(false);
                }                
            }

            OnListChangedEvent?.Invoke(new ListChangedEventArgs(minIndex, maxIndex, lts));
        }

        private void OnSelectChanged()
        {                  
            NavigationItem[] selectedItems = new NavigationItem[] { items[pointer - minIndex] };
            SelectChanged(selectedItems);
        }
   
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

            float verticalOffest = vertical.verticalOffest;
            float horizontalOffest = vertical.horizontalOffest;

            float spacing = vertical.spacing;

            float vh = viewport.rect.size.y;
            float h = tf.rect.size.y;
            int c = Mathf.FloorToInt((vh - verticalOffest) / (h + spacing / 2));

            for (int i = 0; i < c; i++)
            {
                NavigationItem lt = Instantiate<NavigationItem>(item, content);

                float x = 0f + horizontalOffest;
                float y = (h * i + verticalOffest + spacing * i) * (topToBottom ? -1f : 1f);                

                lt.transform.localPosition = new Vector2(x, y);
                lt.SetIndex(i);
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

            float verticalOffest = horizontal.verticalOffest;
            float horizontalOffest = horizontal.horizontalOffest;
            float spacing = horizontal.spacing;

            float vw = viewport.rect.size.x;
            float w = tf.rect.size.x;
            int c = Mathf.FloorToInt((vw - horizontalOffest) / (w + spacing / 2));

            for (int i = 0; i < c; i++)
            {
                NavigationItem lt = Instantiate<NavigationItem>(item, content);

                float x = (w * i + horizontalOffest + spacing * i) * (leftToRight ? 1f : -1f);
                float y = 0f + verticalOffest;

                lt.transform.localPosition = new Vector2(x, y);
                lt.SetIndex(i);
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

            float verticalOffest = grid.verticalOffest;
            float horizontalOffest = grid.horizontalOffest;
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

                    float x = w * j + horizontalOffest + xSpacing * j;
                    float y = h * i + verticalOffest + ySpacing * i;

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
                    lt.SetIndex(index);
                    lt.SetActive(false);
                    items[index] = lt;
                }
            }

            itemCount = rc * cc;
        }
    }
}
