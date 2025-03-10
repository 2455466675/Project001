using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI
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
	public class StaticNavigationGroup : NavigationGroup
    {
        public GroupType groupType;

        [ShowIf("groupType", GroupType.Grid)]
        public int rowCount;
        [ShowIf("groupType", GroupType.Grid)]
        public int columnCount;
        [ShowIf("groupType", GroupType.Grid)]
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
        private bool IsGrid => groupType == GroupType.Grid;

        /// <summary>
        /// 当前选择发生变化时
        /// </summary>
        public event Action<SelectChangedEventArgs> OnSelectChangedEvent;

        public void Init()
        {
            if (items == null || items.Count == 0)
            {
                isInit = false;
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null) 
                {
                    continue;
                }
                items[i].SetIndex(i);
            }

            pointer = 0;
            minIndex = 0;
            maxIndex = items.Count - 1;
            state = GroupState.Exited;
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

        public override void UpdateElementCount(int totalCount)
        {
            base.UpdateElementCount(totalCount);
        }

        public override void OnExit()
        {
            base.OnExit();
            index = null;
        }

        public override bool OnInFocus(bool isRefocus, params int[] indexs)
        {
            if (isRefocus) 
            {
                if (Select(index))
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
            if (IsMultiple)
            {
                MLog.Log("有多个选中元素时不允许此操作,元素数量:", this.index.Length);
                return;
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
                    return;
                }

                bool minus = childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.LowerLeft;
                index = MovePointHorizontal(pointer, minus);
            }
            else if (x > 0)
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
                index = argItems?.Select(item => item.Index).ToArray();

                isSuccess = argItems != null && argItems.Length > 0;
                pointer = argItems != null ? items.IndexOf(argItems[0]) : 0;
            }

            OnSelectChangedEvent?.Invoke(new SelectChangedEventArgs(isSuccess, indexs, argItems));

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
        private void InitGroup()
        {
            items = GetComponentsInChildren<NavigationItem>().ToList();
        }
    }
}

