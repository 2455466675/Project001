using Game.Core;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class ListItem : MonoBehaviour, IGuidable
    {        
        public int Index {get; private set;}
        public bool IsBeSelected {get; private set;}
        public GameObject CurrentGameObject => gameObject;
        public NavigationBox ParentBox {get; private set;}
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }
        public RectTransform TargetTransform
        {
            get
            {
                if (targetTransform != null)
                {
                    return targetTransform;
                }
                return RectTransform;
            }
        }

        [SerializeField]
        private UINotify uiNotify;
        [SerializeField]
        private RectTransform targetTransform;
        private RectTransform rectTransform;
        private ItemDB listItem;

        public void Start()
        {
            uiNotify = GetComponent<UINotify>();
        }

        public void SetParentBox(NavigationBox box)
        {
            ParentBox = box;
        }

        public void Register(ItemDB listItem, int index)
        {
            Index = index;
            this.listItem = listItem;
            OnDatumChange();
        }

        public T GetListItem<T>() where T : ItemDB
        {
            return listItem as T;
        }

        public void OnSubmit()
        {
            if (uiNotify != null)
            {
                uiNotify.OnSubmit(this);
            }
        }

        public void OnSelect()
        {
            if (uiNotify != null)
            {
                uiNotify.OnSelect(this);
            }
        }

        public void OnDeselect()
        {
            if (uiNotify != null)
            {
                uiNotify.OnDeselect(this);
            }
        }

        public void OnMoveToUp()
        {
            if (uiNotify != null)
            {
                uiNotify.OnMoveToUp(this);
            }
        }

        public void OnMoveToDown()
        {
            if (uiNotify != null)
            {
                uiNotify.OnMoveToDown(this);
            }
        }

        public void OnMoveToLeft()
        {
            if (uiNotify != null)
            {
                uiNotify.OnMoveToLeft(this);
            }
        }

        public void OnMoveToRight()
        {
            if (uiNotify != null)
            {
                uiNotify.OnMoveToRight(this);
            }
        }

        protected virtual void OnDatumChange()
        {

        }
    }
}

