using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{

    /// <summary>
    /// 
    /// </summary>
	public class LoopGuidableBox : GuidableBox
    {
        public override int[] CurrIndex => new int[] { pointer };

        [SerializeField]
        private RectTransform viewport;
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private GuidableItemBase item;
        [SerializeField]
        private float topPadding;
        [SerializeField]
        private float bottomPadding;
        [SerializeField]
        private float spacing;

        //private int topGap;
        //private int bottomGap;

        private int totalCount;
        private int minIndex;
        private int maxIndex;
        private int pointer;
        private int itemCount;

        private bool isInit;
        private Dictionary<int, GuidableItemBase> items;

        private event Action<IndexChangedEventArgs> OnIndexChangedEvent;
        private event Action<SelectChangedEventArgs> OnSelectChangedEvent;

        public void Init(Action<IndexChangedEventArgs> indexChangeHandler, Action<SelectChangedEventArgs> selectHandler)
        { 
            if (indexChangeHandler == null || selectHandler == null) 
            {
                isInit = false;
                return;
            }
            OnIndexChangedEvent += indexChangeHandler;
            OnSelectChangedEvent += selectHandler;
            totalCount = -1;
            minIndex = -1;
            maxIndex = -1;
            pointer = -1;
            CreateItems();
            isInit = true;
        }

        public override void Move(MoveType moveType)
        {
            int index = -1;
                
            if (moveType == MoveType.Up)
            {
                index = pointer - 1;
            }
            else if (moveType == MoveType.Down)
            {
                index = pointer + 1;
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

        public override void Select(params int[] indexs)
        {
            if (!isInit) return;
            if (indexs == null || indexs.Length <= 0) return;

            int index = indexs[0];

            bool isSuccess = false;

            if (index < 0 || index >= totalCount)
            {
                isSuccess = false;
            }
            else if (index >= minIndex && index <= maxIndex)
            {
                pointer = index;
                isSuccess = true;
            }
            else if (index < minIndex)
            {
                int i = minIndex - index;

                minIndex -= i;
                maxIndex -= i;
                pointer = minIndex;

                isSuccess = true;
                OnIndexChanged();
            }
            else if (index > maxIndex)
            {
                int i = index - maxIndex;

                minIndex += i;
                maxIndex += i;
                pointer = maxIndex;

                isSuccess = true;
                OnIndexChanged();
            }

            OnSelectChanged(isSuccess);
        }

        public void UpdateTotalCount(int totalCount)
        {
            if (totalCount < 0)
            {
                return;
            }

            if (totalCount == 0)
            {
                minIndex = 0;
                maxIndex = 0;
                pointer = 0;
                this.totalCount = 0;

                foreach (var item in items)
                {
                    item.Value.SetActive(false);
                }

                return;
            }

            if (this.totalCount == totalCount)
            {
                OnIndexChanged();
                return;
            }
            
            this.totalCount = totalCount;

            int oldPointer = pointer;

            if (pointer == 0)
            {
                minIndex = 0;
                maxIndex = Mathf.Min(totalCount - 1, itemCount - 1);
            }
            else
            {
                int i = pointer - minIndex;

                maxIndex = Mathf.Min(totalCount - 1, Mathf.Max(maxIndex, itemCount - 1));                
                minIndex = Mathf.Max(maxIndex - (itemCount - 1), 0);
                pointer = Mathf.Clamp(minIndex + i, minIndex, maxIndex);                
            }

            OnIndexChanged();
            OnSelectChanged(oldPointer != pointer);
        }

        private void CreateItems()
        {
            if (item == null)
            {
                MLog.Log("item is null");
                return;
            }

            float vh = viewport.rect.size.y;

            item.gameObject.SetActive(false);
            RectTransform tf = item.GetComponent<RectTransform>();
            tf.anchorMin = new Vector2(0.5f, 1);
            tf.anchorMax = new Vector2(0.5f, 1);
            tf.pivot = new Vector2(0.5f, 0.5f);

            float h = tf.rect.size.y;
            int c = Mathf.FloorToInt((vh - topPadding - bottomPadding) / (h + spacing / 2));    //¼ÆËã¸öÊý
            items = new Dictionary<int, GuidableItemBase>(c);                 
            for (int i = 0; i < c; i++)
            {   
                GuidableItemBase lt = GoHelper.Instantiate<GuidableItemBase>(item, content);
                lt.RectTransform.anchoredPosition = new Vector2(0, -(topPadding + h / 2 + i * h + i * spacing));
                lt.SetActive(false);                
                items[i] = lt;
            }
            itemCount = items.Count;
        }

        private void OnIndexChanged()
        {
            int length = maxIndex - minIndex + 1;
            GuidableItemBase[] lts = new GuidableItemBase[length];

            foreach (var item in items)
            {
                item.Value.SetActive(item.Key < length);

                if (item.Key >= length)
                {
                    continue;
                }

                lts[item.Key] = item.Value;
            }

            OnIndexChangedEvent?.Invoke(new IndexChangedEventArgs(minIndex, maxIndex, lts));
        }

        private void OnSelectChanged(bool isSuccess)
        {
            OnSelectChangedEvent?.Invoke(new SelectChangedEventArgs(isSuccess, new int[] { pointer }, new GuidableItemBase[] { items[pointer - minIndex] }));
        }
    }
}

