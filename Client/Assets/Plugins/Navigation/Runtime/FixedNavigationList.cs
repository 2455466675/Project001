using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 固定导航列表：Item 数量与数据数量一致、元素预先摆放在场景中的列表。
    /// 支持垂直、水平、网格布局及多选。
    /// </summary>
    public class FixedNavigationList : NavigationList
    {
        public enum ChildAlignment
        {
            UpperLeft,
            UpperRight,
            LowerLeft,
            LowerRight,
        }

        public int rowCount;
        public int columnCount;
        public ChildAlignment childAlignment;

        [SerializeField]
        private List<NavigationItem> items;

        [SerializeField]
        private bool isLoop;

        /// <summary>
        /// 当前选择的索引
        /// </summary>
        private int[] index;
        private bool IsMultiple => index != null && index.Length > 1;
        private bool IsGrid => m_ListType == ListType.Grid;

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

            //Grid 布局的移动运算依赖 rowCount 做除法/取模，必须为正，否则会除零。
            if (IsGrid && rowCount <= 0)
            {
                Debug.LogError("Grid 布局下 rowCount 必须大于 0。");
                isInit = false;
                return;
            }

            items.RemoveAll(x => x == null);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetListIndex(i);
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
                    if (isLoop)
                    {
                        bool minus = childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.UpperRight;
                        index = MovePointVertical(pointer, minus);
                    }
                    else
                    {
                        if ((pointer + 1) % rowCount == 0)
                        {
                            return;
                        }
                        else
                        {
                            index = MovePointVertical(pointer, false);
                        }
                    }
                }
                else
                {
                    index = MovePointVertical(pointer, true);
                }

            }
            else if (v < 0)
            {
                if (IsGrid)
                {
                    if (isLoop)
                    {
                        bool minus = childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.UpperRight;
                        index = MovePointVertical(pointer, !minus);
                    }
                    else
                    {
                        if (pointer % rowCount == 0)
                        {
                            return;
                        }
                        else
                        {
                            index = MovePointVertical(pointer, true);
                        }
                    }
                }
                else
                {
                    index = MovePointVertical(pointer, false);
                }
            }
            else if (h < 0)
            {
                if (!IsGrid)
                {
                    return;
                }

                bool minus = childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.LowerLeft;
                index = MovePointHorizontal(pointer, minus);
            }
            else if (h > 0)
            {
                if (!IsGrid)
                {
                    return;
                }

                bool minus = childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.LowerLeft;
                index = MovePointHorizontal(pointer, !minus);
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

                isSuccess = argItems.Length > 0;
                if (isSuccess)
                {
                    //记录选中元素的列表索引，供重聚焦(refocus)时复原选择；
                    //使用 IndexOfList 而非 IndexOfData，避免数据索引与列表索引语义混用。
                    index = argItems.Select(item => item.IndexOfList).ToArray();
                    pointer = argItems[0].IndexOfList;
                }
                //全部无效时保持 isSuccess=false，index/pointer 维持原值，避免空数组取 [0] 越界。
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
        private int MovePointVertical(int beginIndex, bool minus)
        {
            int index;
            do
            {
                if (minus)
                {
                    index = beginIndex - 1;
                    if (index < minIndex && isLoop)
                    {
                        index = maxIndex;
                    }
                }
                else
                {
                    index = beginIndex + 1;
                    if (index > maxIndex && isLoop)
                    {
                        index = minIndex;
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
        private int MovePointHorizontal(int beginIndex, bool minus)
        {
            int index;

            if (minus)
            {
                index = beginIndex - rowCount;
                if (index < minIndex && isLoop)
                {
                    index = maxIndex + index + 1;
                }
            }
            else
            {
                index = beginIndex + rowCount;
                if (index > maxIndex && isLoop)
                {
                    index = minIndex + index - maxIndex - 1;
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
                    if (a > maxIndex)           //最后一列可能不满，跳过不存在的槽位，防止越界
                    {
                        break;
                    }
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
                    if (q < min)
                    {
                        if (isLoop)
                        {
                            q = max;
                        }
                        else
                        {
                            return -1;  //非循环且越出边界，终止查找，避免负索引/死循环
                        }
                    }
                }
                else
                {
                    q++;
                    if (q > max)
                    {
                        if (isLoop)
                        {
                            q = min;
                        }
                        else
                        {
                            return -1;  //非循环且越出边界，终止查找，避免越界/死循环
                        }
                    }
                }

            } while (true);
        }

        //[Button("Init")]
        private void InitGroup()
        {
            items = GetComponentsInChildren<NavigationItem>().ToList();
            items.RemoveAll(x => x == null);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetListIndex(i);
            }
        }
    }
}
