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
	public class GuidableItemBase : MonoBehaviour, IGuidable, IViewContainer
    {        
        public int Index {get; private set;}
        public bool IsBeSelected {get; private set;}
        public GameObject CurrentGameObject => gameObject;

        private RectTransform rectTransform;
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
        public RectTransform GuidePoint
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

        private ItemDB itemDB;

        [ShowInInspector]
        private List<View> views;

        public void Start()
        {
            UINotify = GetComponent<UINotify>();            
        }

        public void SetActive(bool active)
        {
            CurrentGameObject.SetActive(active);
        }

        public void Register(ItemDB itemDB, int index)
        {
            Index = index;
            this.itemDB = itemDB;
            OnDatumChange();
        }

        public T GetItemDB<T>() where T : ItemDB
        {
            return itemDB as T;
        }

        public void OnSubmit()
        {
            if (UINotify != null)
            {
                UINotify.OnSubmit(this);
            }
        }

        public void OnSelected()
        {
            if (UINotify != null)
            {
                UINotify.OnSelect(this);
            }
        }

        public void OnDeselected()
        {
            if (UINotify != null)
            {
                UINotify.OnDeselect(this);
            }
        }

        public void OnMoveUp()
        {
            if (UINotify != null)
            {
                UINotify.OnMoveUp(this);
            }
        }

        public void OnMoveDown()
        {
            if (UINotify != null)
            {
                UINotify.OnMoveDown(this);
            }
        }

        public void OnMoveLeft()
        {
            if (UINotify != null)
            {
                UINotify.OnMoveLeft(this);
            }
        }

        public void OnMoveRight()
        {
            if (UINotify != null)
            {
                UINotify.OnMoveRight(this);
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

