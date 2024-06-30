using DG.Tweening;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class LoopNavigationBox : NavigationBox
    {
        public ListItem prefab;
        public float topPadding;
        public float bottomPadding;
        public float spacing;

        public RectTransform viewport;
        public RectTransform content;

        public int totalCount;
        public int min;
        public int max;
        public int point;
        public int offest;
        public int gap = 0;

        private bool isInit = false;
        private int itemCount;
        private Dictionary<int, ListItem> items = new();

        private readonly Vector3[] corners = new Vector3[4];
        private Vector2 viewportTop;
        private Vector2 viewportBottom;
        private Action<int, int, ListItem[]> OnIndexChangeHandler;

        public void Init(Action<int, int, ListItem[]> action)
        {
            if (isInit)
            {
                return;
            }
            InitViewport();
            InitItems();
            OnIndexChangeHandler = action;

            point = 0;
            offest = 0;
            totalCount = 0;
            isInit = true;
        }

        public void UpdateTotalCount(int totalCount)
        {
            if (!isInit)
            {
                return;
            }
            if (totalCount < 0)
            {
                return;
            }
            if (totalCount == 0)             
            {
                max = 0;
                point = 0;
                offest = 0;
                this.totalCount = 0;

                foreach (var item in items)
                {
                    item.Value.gameObject.SetActive(false);
                }

                if (IsBeSelected)
                {
                    CommandInvoker.UndoCommand();
                }
                return;
            }

            if (this.totalCount == totalCount)
            {
                return;
            }

            int currIndex = point + offest;
            int limitIndex = totalCount - 1;
            int changeCount = totalCount - this.totalCount;
            this.totalCount = totalCount;

            bool changePoint = false;
     
            max = Mathf.Min(itemCount - 1, limitIndex);

            if (changeCount < 0)
            {
                if (currIndex <= limitIndex)
                {
                    offest = Mathf.Max(0, offest + changeCount);
                    point = currIndex - offest;
                    changePoint = true;
                }
                else if (limitIndex < currIndex)
                {
                    point = max;
                    offest = limitIndex - point;
                    changePoint = true;
                }
            }

            IndexChange();
            if (changePoint && IsBeSelected) 
            {
                Select(point + offest);
            }
        }

        public override void Select(int index)
        {
            if (index < 0 || index >= totalCount)
            {
                return;
            }
            int maxIndex = totalCount - 1;
            currIndex = index;

            if (maxIndex < max || index < max || maxIndex - index >= max)
            {
                point = (maxIndex - index) >= max ? 0 : index;
                offest = (maxIndex - index) >= max ? index : 0;
                AlignAtTop();
            }
            else
            {
                offest = maxIndex - max;
                point = max - (maxIndex - index);
                AlignAtBottom();
            }       
            IndexChange();
            SelectInner(FindOrGreateItem(point));
        }

        public override void MoveUp()
        {      
            if (point == 0)
            {
                if (offest <= 0)
                {
                    return;
                }
                offest -= 1;
                IndexChange();                
            }
            else
            {
                point -= 1;
                if (point == 0)
                {
                    AlignAtTop();
                }                
            }
            if (current) current.OnMoveToUp();
            SelectInner(FindOrGreateItem(point));
        }

        public override void MoveDown()
        {      
            if (point >= max - gap) 
            {                
                if (point >= max && offest >= totalCount - max - 1)
                {                  
                    return;
                }
                AlignAtBottom();
                if (point < max && offest == totalCount - max - 1)
                {
                    point += 1;
                }
                else
                {
                    offest += 1;
                    IndexChange();                    
                }          
            }
            else
            {
                point += 1;
                if (point >= max - gap)
                {
                    AlignAtBottom();
                }
            }
            if (current) current.OnMoveToDown();
            SelectInner(FindOrGreateItem(point));          
        }

        public override void MoveLeft()
        {
            if (!current)
            {
                return;
            }            
            current.OnMoveToLeft();
        }

        public override void MoveRight()
        {
            if (!current)
            {
                return;
            }
            current.OnMoveToRight();
        }

        private void InitViewport()
        {
            if (viewport == null)
            {
                return;
            }

            viewport.GetWorldCorners(corners);
            viewportBottom = RectTransformUtility.WorldToScreenPoint(GameCore.UI.UICamera, corners[0]);
            viewportTop = RectTransformUtility.WorldToScreenPoint(GameCore.UI.UICamera, corners[1]);
        }

        private void InitItems()
        {
            prefab.gameObject.SetActive(false);

            RectTransform tf = prefab.GetComponent<RectTransform>();
            tf.anchorMin = new Vector2(0.5f, 1);
            tf.anchorMax = new Vector2(0.5f, 1);
            tf.pivot = new Vector2(0.5f, 0.5f);
            float prfabHeight = tf.rect.size.y;
            float vHeight = viewport.rect.size.y;
            int c = Mathf.FloorToInt((vHeight - topPadding - bottomPadding) / (prfabHeight + spacing / 2));

            float height = (c - 1) * spacing + c * prfabHeight + topPadding + bottomPadding;
            content.sizeDelta = new Vector2(tf.rect.size.x, height);

            for (int i = 0; i < c; i++)
            {
                ListItem listItem = FindOrGreateItem(i);
                float y = -(topPadding + prfabHeight / 2 + i * prfabHeight + i * spacing);
                listItem.RectTransform.anchoredPosition = new Vector2(0, y);
            }

            itemCount = c;
        }

        private void IndexChange()
        {
            int beginIndex = min + offest;
            int endIndex = max + offest;
            if (beginIndex < 0 || endIndex < 0)
            {
                return;
            }
            int extent = endIndex - beginIndex;
            if (extent < 0)
            {
                return;
            }
            else
            {
                ListItem[] gameObjects = new ListItem[extent + 1];
                foreach (var item in items)
                {
                    if (item.Key > extent)
                    {
                        item.Value.gameObject.SetActive(false);
                        continue;
                    }
                    item.Value.gameObject.SetActive(true);
                    gameObjects[item.Key] = item.Value;
                }
                OnIndexChangeHandler?.Invoke(beginIndex, endIndex, gameObjects);
            }
        }

        private ListItem FindOrGreateItem(int index) 
        {
            if (items.ContainsKey(index))
            {
                return items[index];
            }

            ListItem listItem = GoHelper.Instantiate<ListItem>(prefab, content);
            listItem.gameObject.SetActive(true);
            listItem.SetParentBox(this);

            items.Add(index, listItem);
            return listItem;
        }

        private float GetItemBottomY(ListItem listItem)
        {            
            listItem.RectTransform.GetWorldCorners(corners);
            Vector2 v0 = RectTransformUtility.WorldToScreenPoint(GameCore.UI.UICamera, corners[0]);
            return v0.y;
        }

        private float GetItemTopY(ListItem listItem)
        {
            listItem.RectTransform.GetWorldCorners(corners);
            Vector2 v1 = RectTransformUtility.WorldToScreenPoint(GameCore.UI.UICamera, corners[1]);
            return v1.y;
        }

        private void SelectInner(ListItem listItem)
        {
            currIndex = point + offest;
            current = listItem;
            GameCore.UI.SelectUI(current);
        }

        private void AlignAtTop()
        {
            float topY = GetItemTopY(FindOrGreateItem(0));
            if (topY > viewportTop.y)
            {
                content.anchoredPosition = new Vector2(0, content.anchoredPosition.y - (topY - viewportTop.y) - topPadding);
            }
        }

        private void AlignAtBottom()
        {
            float bottomY = GetItemBottomY(FindOrGreateItem(max));
            if (bottomY < viewportBottom.y)
            {
                content.anchoredPosition = new Vector2(0, content.anchoredPosition.y + viewportBottom.y - bottomY + bottomPadding);
            }
        }
    }
}

