using Game;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
	public class LoopNavigationList : NavigationList
    {
        public int[] CurrIndex => new int[] { pointer };

        [SerializeField]
        private RectTransform viewport;
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private GuidableItem item;
        [SerializeField]
        private float topPadding;
        [SerializeField]
        private float bottomPadding;
        [SerializeField]
        private float spacing;

        private int totalCount;
        private int minIndex;
        private int maxIndex;
        private int pointer;
        private int itemCount;

        private bool isInit;
        private Dictionary<int, GuidableItem> items;

        /// <summary>
        /// 当最大索引和最小索引发生变化时
        /// </summary>
        private event Action<IndexChangedEventArgs> OnIndexChangedEvent;
        /// <summary>
        /// 当前选择发生变化时
        /// </summary>
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
            State = ListState.Exited;
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
            int index = pointer - minIndex;
            if (index < 0 || index >= items.Count)
            {
                return false;
            }
            GuidableItem item = items[index];
            item.OutFocus();
            State = ListState.OutFocus;
            return true;
        }

        public override bool Refocus()
        {
            if (Select(pointer))
            {
                State = ListState.InFocus;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Close()
        {
            pointer = -1;
            State = ListState.Exited;
        }

        public override bool Move(Vector2 dir)
        {
            float y = dir.y;
            int index;
            if (y > 0)
            {
                index = pointer - 1;
            }
            else if (y < 0)
            {
                index = pointer + 1;
            }
            else 
            {
                return false;
            }
 
            return Select(index);
        }

        public override bool Select(params int[] indexs)
        {
            if (!isInit)
            {
                return false;
            }
            if (indexs == null || indexs.Length <= 0)
            {
                return false;
            }

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
            return isSuccess;
        }

        public override void UpdateTotalCount(int totalCount)
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

            if (State == ListState.InFocus)
            {
                OnSelectChanged(oldPointer != pointer);
            }
        }

        private void CreateItems()
        {
            if (item == null)
            {
                MLog.Error("item is null");
                return;
            }

            float vh = viewport.rect.size.y;

            item.gameObject.SetActive(false);
            RectTransform tf = item.GetComponent<RectTransform>();
            tf.anchorMin = new Vector2(0.5f, 1);
            tf.anchorMax = new Vector2(0.5f, 1);
            tf.pivot = new Vector2(0.5f, 0.5f);

            float h = tf.rect.size.y;
            int c = Mathf.FloorToInt((vh - topPadding - bottomPadding) / (h + spacing / 2));    //计算个数
            items = new Dictionary<int, GuidableItem>(c);
            for (int i = 0; i < c; i++)
            {
                GuidableItem lt = GoHelper.Instantiate<GuidableItem>(item, content);
                (lt.transform as RectTransform).anchoredPosition = new Vector2(0, -(topPadding + h / 2 + i * h + i * spacing));
                lt.SetActive(false);
                items[i] = lt;
            }
            itemCount = items.Count;
        }

        private void OnIndexChanged()
        {
            int length = maxIndex - minIndex + 1;
            GuidableItem[] lts = new GuidableItem[length];

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
            OnSelectChangedEvent?.Invoke(new SelectChangedEventArgs(isSuccess, new int[] { pointer }, new GuidableItem[] { items[pointer - minIndex] }));
        }
    }
}

