using Navigation;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NavigationListView : View, INavigation
    {        
        [SerializeField]
        private NavigationListDefine define;
        public bool IsValid => List != null;
        public NavigationListDefine Define => define;
        protected abstract NavigationList List { get; }

        private void OnEnable()
        {
            NavigationListEntity e = Game.UI.GetNavigationListEntity(define);
            e?.OnEnable();
        }

        protected virtual void OnDisable()
        {
            if(List != null) 
            {
                List.Clear();        
            }

            NavigationListEntity e = Game.UI.GetNavigationListEntity(define);
            e?.OnDisable();
        }

        private void OnDestroy()
        {
            List.OnSelectedEvent -= List_OnSelectedEvent;
            List.OnDeselectedEvent -= List_OnDeselectedEvent;
            List.OnSubmitEvent -= List_OnSubmitEvent;
            List.OnMoveEvent -= List_OnMoveEvent;
            List.OnClearItemEvent -= List_OnClearItemEvent;
            List.OnListEmptyEvent -= List_OnListEmptyEvent;
        }

        protected virtual void Register()
        {
            List.OnSelectedEvent += List_OnSelectedEvent;
            List.OnDeselectedEvent += List_OnDeselectedEvent;
            List.OnSubmitEvent += List_OnSubmitEvent;
            List.OnMoveEvent += List_OnMoveEvent;
            List.OnClearItemEvent += List_OnClearItemEvent;
            List.OnListEmptyEvent += List_OnListEmptyEvent;
        }

        private void List_OnListEmptyEvent()
        {
            Game.UI.Back();
        }

        private void List_OnClearItemEvent(NavigationItem obj)
        {
            OnItemUnbindData(obj);
        }

        private void List_OnMoveEvent(float h, float v, NavigationItem obj)
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = Game.UI.GetNavigationListEntity(define);
                if (e == null)
                {
                    return;
                }

                if (h > 0)
                {
                    e.OnMoveRight(item);
                }
                if (h < 0)
                {
                    e.OnMoveLeft(item);
                }
                if (v > 0)
                {
                    e.OnMoveUp(item);
                }
                if (v < 0)
                {
                    e.OnMoveDown(item);
                }
            }
        }

        private void List_OnSubmitEvent(NavigationItem obj)
        {
            if (obj != null && obj is GameNavigationItem item) 
            {
                NavigationListEntity e = Game.UI.GetNavigationListEntity(define);
                e?.OnSubmit(item);
            }
        }

        private void List_OnDeselectedEvent(NavigationItem obj)
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = Game.UI.GetNavigationListEntity(define);
                e?.OnDeselect(item);
            }
        }

        private void List_OnSelectedEvent(NavigationItem obj)
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = Game.UI.GetNavigationListEntity(define);
                e?.OnSelect(item);
            }
        }

        protected void OnItemBindData(NavigationItem obj) 
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = Game.UI.GetNavigationListEntity(define);
                e?.OnBindData(item);

                obj.OnRefreshEvent += Obj_OnRefreshEvent;
            }
        }

        protected void OnItemUnbindData(NavigationItem obj)
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = Game.UI.GetNavigationListEntity(define);
                e?.OnUnbindData(item);

                obj.OnRefreshEvent -= Obj_OnRefreshEvent;
            }
        }

        private void Obj_OnRefreshEvent(NavigationItem obj)
        {
            OnItemRefresh(obj);
        }

        protected void OnItemRefresh(NavigationItem obj) 
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = Game.UI.GetNavigationListEntity(define);
                e?.OnRefresh(item);
            }
        }

        public abstract void UpdateData(INavigationItemData[] data);

        public void Move(float h, float v)
        {
            if (List != null) 
            {
                List.Move(h, v);   
            }            
        }

        public void Submit()
        {
            if (List != null) 
            {
                List.Submit();
            }
        }

        public bool InFocus(bool isRefocus, int[] indexs = null)
        {
            if (List != null)
            {
                return List.InFocus(isRefocus, indexs);               
            }
            else
            {
                return false;
            }
        }

        public void OutFocus()
        {
            if (List != null)
            {
                List.OutFocus();
            }
        }

        public void Exit()
        {
            if (List != null)
            {
                List.Exit();
            }
        }
    }
}
