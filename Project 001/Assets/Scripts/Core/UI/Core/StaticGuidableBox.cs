using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Game.UI
{
    public enum ListType 
    {
        Vertical    = 0,
        Horizontal  = 1,
        Grid        = 2,
    }

    public enum ChildAlignment 
    {
        UpperLeft   = 0,
        UpperRight  = 1,
        LowerLeft   = 2, 
        LowerRight  = 3,
    }

    /// <summary>
    /// 
    /// </summary>
	public class StaticGuidableBox : GuidableBox
    {
        public override int[] CurrIndex => index;

        public ListType listType;

        [ShowIf("listType", ListType.Grid)]
        public int rowCount;
        [ShowIf("listType", ListType.Grid)]
        public int columnCount;
        [ShowIf("listType", ListType.Grid)]
        public ChildAlignment childAlignment;

        public List<GuidableItemBase> items;
        public bool isLoop;

        private int minIndex;
        private int maxIndex;
        private int pointer;
        private bool isInit;

        private int[] index;
        private bool IsMultiple => index != null && index.Length > 1;
        private bool IsGrid => listType == ListType.Grid;

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

            OnSelectChangedEvent += selectHandler;
            pointer = 0;
            minIndex = 0;
            maxIndex = items.Count - 1;
            isInit = true;
        }

        public GuidableItemBase GetItem(int index)
        {
            if (items == null || items.Count == 0)
            {
                return null;
            }
            if(index < 0 || index >= items.Count) 
            {
                return null;
            }
            return items[index];
        }

        public override void Select(params int[] index)
        {
            if (!isInit) return;
            if (items == null || items.Count <= 0) return;
            if (index == null || index.Length <= 0) return;

            bool isSuccess = true;

            for (int i = 0; i < index.Length; i++)
            {
                if (index[i] < minIndex || index[i] > maxIndex)
                {
                    isSuccess = false;
                    break;
                }
            }

            GuidableItemBase[] argItems = null;
            if (isSuccess)
            {
                this.index = index;
                argItems = new GuidableItemBase[index.Length];
                for (int i = 0; i < index.Length; i++)
                {
                    argItems[i] = items[index[i]];
                }
                pointer = index[0];
            }

            OnSelectChangedEvent?.Invoke(new SelectChangedEventArgs(isSuccess, index, argItems));
        }

        public override void Move(MoveType moveType)
        {
            if (IsMultiple)
            {
                MLog.Log("有多个选中元素时不允许此操作,元素数量:", this.index.Length);
                return;
            }

            int index = -1;
            if (moveType == MoveType.Up)
            {
                bool minus = !IsGrid || childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.UpperRight;
                index = MovePointVertical(pointer, minus);
            }
            else if (moveType == MoveType.Down)
            {
                bool minus = !IsGrid || childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.UpperRight;
                index = MovePointVertical(pointer, !minus);
            }
            else if (moveType == MoveType.Left)
            {
                if (!IsGrid)
                {
                    return;
                }

                bool minus = childAlignment == ChildAlignment.UpperLeft || childAlignment == ChildAlignment.LowerLeft;
                index = MovePointHorizontal(pointer, minus);
            }
            else if (moveType == MoveType.Right)
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

            Select(index);
        }

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

            } while (true);
        }

        private int MovePointHorizontal(int beginIndex, bool minus)
        {
            int index;

            if (minus)
            {
                index = beginIndex - rowCount;
                if (index < minIndex && isLoop)
                {
                    index = maxIndex - index + 1;
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
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    int a = q * rowCount + i;
                    if (items[a].IsValid)
                    {
                        return a;
                    }
                }

                if (q == b)
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
    }
}

