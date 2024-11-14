using Game;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Navigation
{
    public enum ChildAlignment
    {
        UpperLeft,
        UpperRight,
        LowerLeft,
        LowerRight,
    }

    /// <summary>
    /// 元素是一直存在的，不会创建元素
    /// </summary>
	public class StaticNavigationList : NavigationList
    {
        public int[] CurrIndex => index;

        public ListType listType;

        [ShowIf("listType", ListType.Grid)]
        public int rowCount;
        [ShowIf("listType", ListType.Grid)]
        public int columnCount;
        [ShowIf("listType", ListType.Grid)]
        public ChildAlignment childAlignment;

        public List<GuidableItem> items;
        public bool isLoop;

        /// <summary>
        /// 当前显示的数据组的起始索引
        /// </summary>
        private int minIndex;
        /// <summary>
        /// 当前显示的数据组的结束索引
        /// </summary>
        private int maxIndex;
        /// <summary>
        /// 当前指针位置
        /// </summary>
        private int pointer;
        private bool isInit;

        private int[] index;
        private bool IsMultiple => index != null && index.Length > 1;
        private bool IsGrid => listType == ListType.Grid;

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
            if (items == null || items.Count == 0)
            {
                isInit = false;
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetIndex(i);
            }

            OnSelectChangedEvent += selectHandler;
            pointer = 0;
            minIndex = 0;
            maxIndex = items.Count - 1;
            State = ListState.Exited;
            isInit = true;
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

        public override void UpdateTotalCount(int totalCount)
        {
            base.UpdateTotalCount(totalCount);
            UpateTime++;
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
                if (indexs[i] < minIndex || indexs[i] > maxIndex)
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
            SelectedItems = argItems;
            OnSelectChangedEvent?.Invoke(new SelectChangedEventArgs(isSuccess, indexs, argItems));
            return isSuccess;
        }

        public override bool Move(Vector2 dir)
        {
            if (IsMultiple)
            {
                MLog.Log("有多个选中元素时不允许此操作,元素数量:", this.index.Length);
                return false;
            }

            float x = dir.x;
            float y = dir.y;

            int index = -1;
            if (y > 0)
            {
                bool minus = !IsGrid || childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.UpperRight;
                index = MovePointVertical(pointer, minus);
            }
            else if (y < 0)
            {
                bool minus = !IsGrid || childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.UpperRight;
                index = MovePointVertical(pointer, !minus);
            }
            else if (x < 0)
            {
                if (!IsGrid)
                {
                    return false;
                }

                bool minus = childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.LowerLeft;
                index = MovePointHorizontal(pointer, minus);
            }
            else if (x > 0)
            {
                if (!IsGrid)
                {
                    return false;
                }

                bool minus = childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.LowerLeft;
                index = MovePointHorizontal(pointer, !minus);
            }

            if (index < minIndex || index > maxIndex)
            {
                return false;
            }

            if (index == pointer)
            {
                return false;
            }

            return Select(index);
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
                    MLog.Error("列表索引出现了异常，没有可用元素。index:" + index);
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

            int min = 0;
            int max = maxIndex / rowCount;

            int b = beginIndex / rowCount;
            int q = index / rowCount;

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
                    MLog.Error("列表索引出现了异常，没有可用元素。index:" + index);
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

        [Button("Init")]
        private void Init()
        {
            items = GetComponentsInChildren<GuidableItem>().ToList();
        }
    }
}

