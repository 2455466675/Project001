using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class FixedNavigationList : NavigationList
    {
        private enum GridAlignment
        {
            UpperLeft,
            UpperRight,
            LowerLeft,
            LowerRight,
        }

        private enum VerticalAlignment
        {
            TopToBottom,
            BottomToTop,
        }

        private enum HorizontalAlignment
        {
            LeftToRight,
            RightToLeft,
        }

        [SerializeField]
        [ShowIfEnum("m_ListType", ListType.Grid)]
        private int rowCount;
        [SerializeField]
        [ShowIfEnum("m_ListType", ListType.Grid)]
        private int columnCount;
        [SerializeField]
        [ShowIfEnum("m_ListType", ListType.Grid)]
        private GridAlignment gridAlignment;

        [SerializeField]
        [ShowIfEnum("m_ListType", ListType.Vertical)]
        private VerticalAlignment verticalAlignment;

        [SerializeField]
        [ShowIfEnum("m_ListType", ListType.Horizontal)]
        private HorizontalAlignment horizontalAlignment;

        [SerializeField]
        private bool isLoop;

        [SerializeField]
        private List<NavigationItem> items;

        /// <summary>
        /// 当前选择的索引
        /// </summary>
        private int[] index;
        private bool IsMultiple => index != null && index.Length > 1;
        private bool IsGrid => m_ListType == ListType.Grid;
        private bool IsVertical => m_ListType == ListType.Vertical;
        private bool IsHorizontal => m_ListType == ListType.Horizontal;

        public override void Init()
        {
            if (isInit)
            {
                Debug.LogWarning("repeat init");
                return;
            }

            if (items == null || items.Count == 0)
            {
                isInit = false;
                return;
            }

            pointer = 0;
            minIndex = 0;
            maxIndex = items.Count - 1;
            state = ListState.Closed;
            isInit = true;
        }

        public override void UpdateDataCount(int count) 
        {
            if (count <= 0) 
            {
                return;
            }

            if (items == null || items.Count == 0) 
            {
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                NavigationItem item = items[i];
                if (i < count) 
                {
                    SetItemDataIndex(item, i);
                }
                else
                {
                    SetItemDataIndex(item, -1);
                }
            }
        }

        public override NavigationItem GetNavigationItem(int index)
        {
            if (index >= 0 &&  index < items.Count)
            {
                return items[index];
            }
            else
            {
                return null;
            }
        }

        protected override void OnExit()
        {
            base.OnExit();
            index = null;
        }

        protected override bool OnInFocus(bool isRefocus, int[] indexs)
        {
            int[] indexArray = isRefocus ? index : indexs;
            if (Select(indexArray))
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
            if (!isInit)
            {
                Debug.LogError("not init");
                return;
            }

            if (IsMultiple)
            {
                Debug.LogError("有多个选中元素时不允许此操作,元素数量:" + this.index.Length);
                return;
            }

            int index = -1;
            if (v > 0)
            {
                if (IsGrid)
                {
                    bool minus = gridAlignment == GridAlignment.UpperLeft || gridAlignment == GridAlignment.UpperRight;
                    index = MovePointVertical(pointer, minus, columnCount);
                }

                if (IsVertical)
                {
                    bool minus = verticalAlignment == VerticalAlignment.TopToBottom;
                    index = MovePointVertical(pointer, minus, 1);
                }               
            }
            else if (v < 0)
            {
                if (IsGrid)
                {
                    bool minus = gridAlignment == GridAlignment.LowerLeft || gridAlignment == GridAlignment.LowerRight;
                    index = MovePointVertical(pointer, minus, columnCount);
                }

                if (IsVertical)
                {
                    bool minus = verticalAlignment == VerticalAlignment.BottomToTop;
                    index = MovePointVertical(pointer, minus, 1);
                }
            }
            else if (h < 0)
            {
                if (IsGrid)
                {
                    bool minus = gridAlignment == GridAlignment.UpperLeft || gridAlignment == GridAlignment.LowerLeft;
                    index = MovePointHorizontal(pointer, minus, 1);
                }
                
                if (IsHorizontal)
                {
                    bool minus = horizontalAlignment == HorizontalAlignment.LeftToRight;
                    index = MovePointHorizontal(pointer, minus, 1);
                }

            }
            else if (h > 0)
            {
                if (IsGrid)
                {
                    bool minus = gridAlignment == GridAlignment.UpperRight || gridAlignment == GridAlignment.LowerRight;
                    index = MovePointHorizontal(pointer, minus, 1);
                }

                if (IsHorizontal)
                {
                    bool minus = horizontalAlignment == HorizontalAlignment.RightToLeft;
                    index = MovePointHorizontal(pointer, minus, 1);
                }

            }

            if (index < minIndex || index > maxIndex)
            {
                return;
            }

            if (index == pointer)
            {
                return;
            }

            Select(new int[] { index });
        }

        private bool Select(int[] indexs)
        {
            if (!isInit)
            {
                Debug.LogError("not init");
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
                if (indexs[i] < minIndex || indexs[i] > maxIndex)
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

                //将无效的元素剔除
                argItems = argItems.Where(item => item.IsValid).ToArray();
                index = argItems?.Select(item => item.IndexOfList).ToArray();

                isSuccess = argItems != null && argItems.Length > 0;
                pointer = argItems != null ? items.IndexOf(argItems[0]) : 0;
            }

            if (isSuccess)
            {
                SelectChanged(argItems);
            }

            return isSuccess;
        }

        /// <summary>
        /// 纵向移动
        /// </summary>
        /// <param name="beginIndex">起始索引</param>
        /// <param name="minus">从起始索引开始减去</param>
        /// <returns></returns>
        private int MovePointVertical(int beginIndex, bool minus, int c)
        {
            int index;
            do
            {
                if (minus)
                {
                    index = beginIndex - c;

                    if (IsVertical)
                    {
                        if (index < minIndex && isLoop)
                        {
                            index = maxIndex;
                        }
                    }

                    if (IsGrid)
                    {
                        if (index < minIndex && isLoop)
                        {
                            index = beginIndex + (rowCount - 1) * c;
                        }
                    }
                }
                else
                {
                    index = beginIndex + c;
                    if (IsVertical)
                    {
                        if (index > maxIndex && isLoop)
                        {
                            index = minIndex;
                        }
                    }

                    if (IsGrid)
                    {
                        if (index > maxIndex && isLoop)
                        {
                            index = beginIndex - (rowCount - 1) * c;
                        }
                    }
                }

                if (index == beginIndex)
                {
                    Debug.LogError("列表索引出现了异常，没有可用元素。index:" + index);
                    return -1;
                }
                else if (index < minIndex || index > maxIndex)
                {
                    return -1;
                }

                if (items[index].IsValid)
                {
                    return index;
                }
                else
                {
                    beginIndex = index;
                }

            } while (true);
        }

        /// <summary>
        /// 横向移动
        /// </summary>
        /// <param name="beginIndex">起始索引</param>
        /// <param name="minus">从起始索引开始减去</param>
        /// <returns></returns>
        private int MovePointHorizontal(int beginIndex, bool minus, int c)
        {
            int index;

            if (minus)
            {
                index = beginIndex - c;

                if (IsHorizontal)
                {
                    if (index < minIndex && isLoop)
                    {
                        index = maxIndex;
                    }
                }

                if (IsGrid)
                {
                    if ((index + c) % columnCount == 0 && isLoop)
                    {
                        index = beginIndex + (columnCount - 1) * c;
                    }
                }
            }
            else
            {
                index = beginIndex + c;
                if (IsVertical)
                {
                    if (index > maxIndex && isLoop)
                    {
                        index = minIndex;
                    }
                }

                if (IsGrid)
                {
                    if (index % columnCount == 0 && isLoop)
                    {
                        index = beginIndex - (columnCount - 1) * c;
                    }
                }
            }

            if (index < minIndex || index > maxIndex)
            {
                return -1;
            }

            if (items[index].IsValid)
            {
                return index;
            }

            //无效索引，进行补偿。
            //每列开始，从上往下，查找一个有效的。

            int min = 0;
            int max = maxIndex / rowCount;  //最大行数。

            int b = beginIndex / rowCount;  //从第几列开始
            int q = index / rowCount;       //当前无效索引所在列

            do
            {
                for (int i = 0; i <= rowCount - 1; i++)
                {
                    int a = q * rowCount + i;   //同一行相邻没有元素时，则从该列的从上往下选择可用的
                    if (items[a].IsValid)
                    {
                        return a;
                    }
                }

                if (q == b) //已经判断了一圈
                {
                    Debug.LogError("列表索引出现了异常，没有可用元素。index:" + index);
                    return -1;
                }

                if (minus)
                {
                    q--;
                    if (q < min && isLoop)
                    {
                        q = max;
                    }
                }
                else
                {
                    q++;
                    if (q > max && isLoop)
                    {
                        q = min;
                    }
                }

            } while (true);
        }

#if UNITY_EDITOR

        [ContextMenu("Init")]
        private void InitGroup()
        {
            items = GetComponentsInChildren<NavigationItem>().ToList();
            items.RemoveAll(x => x == null);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetListIndex(i);
            }
        }
#endif
    }
}
