using System;
using System.Collections.Generic;


namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class StaticGuidableBox : GuidableBox
    {
        public override int CurrIndex => pointer;

        public List<GuidableItemBase> items;
        public bool isLoop;

        private int minIndex;
        private int maxIndex;
        private int pointer;
        private bool isInit;

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

        public override void Select(int index)
        {
            if (!isInit) return;
            if (items == null || items.Count <= 0) return;

            bool isSuccess;
            if (index < minIndex || index > maxIndex)
            {
                isSuccess = false;
            }
            else
            {
                isSuccess = true;
                pointer = index;
            }

            OnSelectChangedEvent?.Invoke(new SelectChangedEventArgs(isSuccess, pointer, items[pointer]));
        }

        public override void Move(MoveType moveType)
        {
            int index = -1;
            if (moveType == MoveType.Up)
            {
                if (!isLoop)
                {
                    index = pointer == minIndex ? -1 : pointer - 1;
                }
                else
                {
                    index = pointer > minIndex ? pointer - 1 : maxIndex;                    
                }
            }
            else if (moveType == MoveType.Down)
            {
                if (!isLoop)
                {
                    index = pointer == maxIndex ? -1 : pointer + 1;
                }
                else
                {
                    index = pointer < maxIndex ? pointer + 1 : 0;                        
                }
            }
            else if (moveType == MoveType.Left)
            {
                return;
            }
            else if (moveType == MoveType.Right)
            {
                return;
            }

            Select(index);
        }
    }
}

