using Game.Core;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public interface IViewContainer
    {
        View GetView(string vUid);
        T GetView<T>(string vUid) where T : View;
    }

    /// <summary>
    /// 
    /// </summary>
	public class ListItem : MonoBehaviour, IGuidable, IViewContainer
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
        private UINotify UINotify;
        [SerializeField]
        private RectTransform targetTransform;
        private RectTransform rectTransform;
        private ItemDB listItem;

        [ReadOnly]
        [ShowInInspector]
        private List<View> views;

        public void Start()
        {
            UINotify = GetComponent<UINotify>();            
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
            if (UINotify != null)
            {
                UINotify.OnSubmit(this);
            }
        }

        public void OnSelect()
        {
            if (UINotify != null)
            {
                UINotify.OnSelect(this);
            }
        }

        public void OnDeselect()
        {
            if (UINotify != null)
            {
                UINotify.OnDeselect(this);
            }
        }

        public void OnMoveToUp()
        {
            if (UINotify != null)
            {
                UINotify.OnMoveToUp(this);
            }
        }

        public void OnMoveToDown()
        {
            if (UINotify != null)
            {
                UINotify.OnMoveToDown(this);
            }
        }

        public void OnMoveToLeft()
        {
            if (UINotify != null)
            {
                UINotify.OnMoveToLeft(this);
            }
        }

        public void OnMoveToRight()
        {
            if (UINotify != null)
            {
                UINotify.OnMoveToRight(this);
            }
        }

        protected virtual void OnDatumChange()
        {

        }

        public View GetView(string vUid)
        {
            if (views == null)
            {
                InitViews();
            }
            if (views == null || views.Count <= 0)
            {
                return null;
            }            
            return views.Find(v => v.vUid == vUid);
        }

        public T GetView<T>(string vUid) where T : View
        {
            if (views == null)
            {
                InitViews();
            }
            if (views == null || views.Count <= 0)
            {
                return null;
            }
            return views.Find(v => v.vUid == vUid) as T;
        }

        [Button("InitViews")]
        private void InitViews()
        {
            views ??= new List<View>();        
            views.Clear();
            View[] cViews = GetComponentsInChildren<View>();
            if (cViews == null || cViews.Length <= 0)
            {
                return;
            }
            foreach (View view in cViews) 
            {
                if (string.IsNullOrEmpty(view.vUid))
                {
                    MLog.Warn($"对应视图没有填写vUid,游戏物体:{view.gameObject.name}");
                    continue;
                }

                if (views.Find(v => v.vUid == view.vUid))
                {
                    MLog.Warn($"重复的vUid:{view.vUid}");
                    continue;
                }
                views.Add(view);
            }
        }
    }
}

