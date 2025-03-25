using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class FluidNavigationList : NavigationList
    {
        [Serializable]
        private class Vertical
        {
            public enum Alignment
            {
                TopToBottom,
                BottomToTop,
            }

            public float horizontalOffest;
            public float verticalOffest;
            public float spacing;
            public Alignment alignment;
        }

        [Serializable]
        private class Horizontal 
        {
            public enum Alignment
            {
                LeftToRight,
                RightToLeft,
            }

            public float horizontalOffest;
            public float verticalOffest;
            public float spacing;
            public Alignment alignment;
        }

        [Serializable]
        private class Grid 
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
        }

        /// <summary>
        /// 当列表发生变化时
        /// </summary>
        public event Action<ListChangedEventArgs> OnListChangedEvent;

        [SerializeField]
        private NavigationItem item;
        [SerializeField]
        private RectTransform viewport;
        [SerializeField]
        private RectTransform content;

        private bool Isvertical => listType == ListType.Vertical;
        private bool IsHorizontal => listType == ListType.Horizontal;
        private bool IsGrid => listType == ListType.Grid;

        [SerializeField]
        [ShowIf("listType", ListType.Vertical)]
        private Vertical vertical;

        [SerializeField]
        [ShowIf("listType", ListType.Horizontal)]
        private Horizontal horizontal;

        [SerializeField]
        [ShowIf("listType", ListType.Grid)]
        private Grid grid;

        private int totalCount;
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
                    if ((maxIndex + 1) % grid.rowCount == 0) 
                    {
                        minIndex = Mathf.Max(maxIndex - (grid.rowCount * grid.columnCount) + 1, 0);
                    }
                    else
                    {
                        minIndex = Mathf.Max(maxIndex - ((grid.rowCount - 1) * grid.columnCount + (maxIndex + 1) % grid.rowCount) + 1, 0);
                    }

                    pointer = Mathf.Clamp(pointer, minIndex, maxIndex);

                    OnListChanged();
                    if (state == ListState.InFocused && (oldPointer != pointer || Mathf.Abs(oldMin - minIndex) >= grid.rowCount))
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
                if (v > 0)
                {
                    index = pointer + ((vertical.alignment == Vertical.Alignment.TopToBottom) ? -1 : 1);
                }
                else if (v < 0)
                {                    
                    index = pointer + ((vertical.alignment == Vertical.Alignment.TopToBottom) ? 1 : -1);
                }
            }

            if (IsHorizontal) 
            {
                if (h > 0) 
                {
                    index = pointer + ((horizontal.alignment == Horizontal.Alignment.LeftToRight) ? 1 : -1);
                }
                else if(h < 0)
                {
                    index = pointer + ((horizontal.alignment == Horizontal.Alignment.LeftToRight) ? -1 : +1);
                }
            }

            if (IsGrid) 
            {
                bool upperLeft = grid.alignment == Grid.Alignment.UpperLeft;
                bool upperRight = grid.alignment == Grid.Alignment.UpperRight;
                bool lowerLeft = grid.alignment == Grid.Alignment.LowerLeft;
                bool lowerRight = grid.alignment == Grid.Alignment.LowerRight;

                if (v > 0)
                {
                    if (upperLeft || upperRight) 
                    {
                        index = pointer - grid.rowCount;
                    }
       
                    if (lowerLeft || lowerRight) 
                    {
                        index = pointer + grid.rowCount;
                    }
                }
                else if (v < 0)
                {
                    if (upperLeft || upperRight)
                    {
                        index = pointer + grid.rowCount;
                    }

                    if (lowerLeft || lowerRight)
                    {
                        index = pointer - grid.rowCount;
                    }
                }

                if (h > 0)
                {
                    if (upperLeft || lowerLeft)
                    {                      
                        if ((pointer + 1) % grid.rowCount > 0) 
                        {
                            index = pointer + 1;
                        }                        
                    }

                    if (upperRight || lowerRight)
                    {
                        if ((pointer) % grid.rowCount > 0)
                        {
                            index = pointer - 1;
                        }
                    }
                }
                else if (h < 0)
                {
                    if (upperLeft || lowerLeft)
                    {
                        if ((pointer) % grid.rowCount > 0)
                        {
                            index = pointer - 1;
                        }
                    }

                    if (upperRight || lowerRight)
                    {
                        if ((pointer + 1) % grid.rowCount > 0)
                        {
                            index = pointer + 1;
                        }
                    }
                }
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
                    minIndex -= grid.rowCount;
                    if((maxIndex + 1) % grid.rowCount == 0) 
                    {
                        maxIndex -= grid.rowCount;
                    }
                    else 
                    {
                        maxIndex -= (maxIndex + 1) % grid.rowCount;
                    }
                }
                else
                {
                    minIndex -= 1;
                    maxIndex -= 1;
                }

                pointer = index;        
                OnListChanged();
                return true;
            }

            if (index > maxIndex)
            {
                if (IsGrid)
                {
                    minIndex += grid.rowCount;
                    maxIndex += grid.rowCount;
                    maxIndex = Mathf.Min(totalCount - 1, maxIndex);
                }
                else
                {
                    minIndex += 1;
                    maxIndex += 1;
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

                    int index = rc * i + j;
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
