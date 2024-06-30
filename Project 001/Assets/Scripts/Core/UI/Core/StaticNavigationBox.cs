using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class StaticNavigationBox : NavigationBox
	{
        public List<ListItem> items;
        public bool isLoop;

        public void Awake()
        {
            if (items == null || items.Count <= 0) 
            {
                return;
            }
            for (int i = 0; i < items.Count; i++) 
            {
                items[i].SetParentBox(this);
            }
        }

        public override void Select(int index)
        {
            if (items == null || items.Count <= 0) return;
            if (index < 0 || index >= items.Count) return;

            currIndex = index;
            current = items[index];
            GameCore.UI.SelectUI(current);            
        }

        public override void MoveUp()
        {
            if (items == null || items.Count <= 0)
            {
                return;
            }

            items[currIndex].OnMoveToUp();

            int index = NextIndex(MoveType.Up);
            if (index < 0)
            {
                return;
            }
            Select(index);
        }

        public override void MoveDown()
        {
            if (items == null || items.Count <= 0)
            {
                return;
            }
            
            if (currIndex < 0 || currIndex >= items.Count)
            {
                return;
            }

            items[currIndex].OnMoveToDown();

            int index = NextIndex(MoveType.Down);
            if (index < 0)
            {
                return;
            }
            Select(index);
        }

        public override void MoveLeft()
        {
            if (items == null || items.Count <= 0)
            {
                return;
            }

            if (currIndex < 0 || currIndex >= items.Count)
            {
                return;
            }

            items[currIndex].OnMoveToLeft();

            int index = NextIndex(MoveType.Left);
            if (index < 0)
            {
                return;
            }
            Select(index);
        }

        public override void MoveRight()
        {
            if (items == null || items.Count <= 0)
            {
                return;
            }

            if (currIndex < 0 || currIndex >= items.Count)
            {
                return;
            }

            items[currIndex].OnMoveToRight();

            int index = NextIndex(MoveType.Right);
            if (index < 0)
            {
                return;
            }
            Select(index);
        }

        public ListItem GetListItem(int index)
        {
            if (items == null || items.Count <= 0)
            {
                return null;
            }
            if (index < 0 || index >= items.Count)
            {
                return null;
            }
            return items[index];
        }

        private int NextIndex(MoveType moveType)         
        {           
            if (navigationType == NavigationType.Vertical)
            {
                if (moveType == MoveType.Up)
                {
                    if (currIndex == 0 && !isLoop)
                    {
                        return -1;
                    }
                    return currIndex > 0 ? currIndex - 1 : items.Count - 1;
                }

                if(moveType == MoveType.Down)
                {
                    if (currIndex == items.Count - 1 && !isLoop)
                    {
                        return -1;
                    }
                    return currIndex < items.Count - 1 ? currIndex + 1 : 0;
                }
               
                return -1;
            }

            if (navigationType == NavigationType.Horizontal)
            {
                if (moveType == MoveType.Left)
                {
                    if (currIndex == 0 && !isLoop)
                    {
                        return -1;
                    }
                    return currIndex > 0 ? currIndex - 1 : items.Count - 1;
                }

                if (moveType == MoveType.Right)
                {
                    if (currIndex == items.Count - 1 && !isLoop)
                    {
                        return -1;
                    }
                    return currIndex < items.Count - 1 ? currIndex + 1 : 0;
                }
                return -1;
            }

            return -1;
        }
    }
}

