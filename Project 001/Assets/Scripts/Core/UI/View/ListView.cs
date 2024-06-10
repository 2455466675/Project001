using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public enum ListViewState
    {
        /// <summary>
        /// ¾Û½¹
        /// </summary>
        InFocus  = 1, 
        /// <summary>
        /// Ê§½¹
        /// </summary>
        OutFocus = 2,
        /// <summary>
        /// Òþ²Ø
        /// </summary>
        Hidden   = 3,
    }

    /// <summary>
    /// 
    /// </summary>
	public class ListView : View, IGuidable
	{
        public NavigationBox box;
        public ListDB ListDB => IsValid ? MainDB as ListDB : null;
        public ListViewState ViewState {get; private set;}

        public bool IsBeSelected {get; private set;}

        public GameObject CurrentGameObject => gameObject;

        public RectTransform TargetTransform => transform as RectTransform;

        public override void OnDisable()
        {
            base.OnDisable();
            ViewState = ListViewState.Hidden;
        }

        public void OnSubmit()
        {

        }

        public void OnSelect()
        {
            ViewState = ListViewState.InFocus;
            box.SelectDefault();
        }

        public void OnDeselect()
        {
            ViewState = ListViewState.OutFocus;
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

