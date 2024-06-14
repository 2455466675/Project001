using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public enum ListViewState
    {
        /// <summary>
        /// 聚焦
        /// </summary>
        InFocus  = 1, 
        /// <summary>
        /// 失焦
        /// </summary>
        OutFocus = 2,
        /// <summary>
        /// 隐藏
        /// </summary>
        Hidden   = 3,
    }

    /// <summary>
    /// 
    /// </summary>
	public class ListView : View, IGuidable
	{
        public NavigationBox box;     
        public bool IsBeSelected {get; private set;}
        public ListViewState ViewState {get; private set;}
        public GameObject CurrentGameObject => gameObject;
        public RectTransform TargetTransform => transform as RectTransform;
        public ListDB ListDB => IsValid ? MainDB as ListDB : null;
        public int Count => items != null ? items.Count : 0;

        protected List<ItemDB> items;

        private void Awake()
        {
            if(box == null)
            {
                box = gameObject.GetComponent<NavigationBox>();
            }
            if(box != null)
            {
                box.SetView(this);
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            ViewState = ListViewState.Hidden;
            IsBeSelected = false;
        }

        public override void UpdateView()
        {
            Query();
        }

        private void Query()
        {
            if (ListDB == null || ListDB.Count <= 0)
            {
                items?.Clear();
                return;
            }
            items = ListDB.Where(a => a.Filter()).OrderBy(a => a).ToList();
        }

        public void OnSubmit()
        {

        }

        public void OnSelect()
        {
            if (ViewState == ListViewState.Hidden)
            {
                box.SelectDefault();
            }
            else
            {
                //TODO 选择失焦之前选择的
                Debug.Log("选择失焦之前选择的");
                box.SelectDefault();
            }
            ViewState = ListViewState.InFocus;
            IsBeSelected = true;
        }

        public void OnDeselect()
        {
            ViewState = ListViewState.OutFocus;
            IsBeSelected = false;
        }

        public void OnMoveToUp()
        {
            if (box == null) 
            {
                return;
            }
            box.MoveUp();                
        }

        public void OnMoveToDown()
        {
            if (box == null)
            {
                return;
            }
            box.MoveDown();
        }

        public void OnMoveToLeft()
        {
            if (box == null)
            {
                return;
            }
            box.MoveLeft();
        }

        public void OnMoveToRight()
        {
            if (box == null)
            {
                return;
            }
            box.MoveRight();
        }
    }
}

