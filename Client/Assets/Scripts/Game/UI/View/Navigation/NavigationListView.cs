using Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NavigationListView : View, INavigation
    {        
        [ReadOnly]
        [SerializeField]
        private int guid;

        [SerializeField]
        private NavigationListDefine define;
        public bool IsValid => List != null;
        public NavigationListDefine Define => define;
        protected abstract NavigationList List { get; }

        public void Bind(int guid) 
        {
            this.guid = guid;            
        }

        private void OnEnable()
        {
            NavigationListEntity e = GameWorld.FindEntity<NavigationListEntity>(guid);
            e?.OnEnable();
        }

        private void OnDisable()
        {
            if(List != null) 
            {
                List.Clear();            
            }

            NavigationListEntity e = GameWorld.FindEntity<NavigationListEntity>(guid);
            e?.OnDisable();
        }

        private void OnDestroy()
        {
            guid = -1;
        }

        protected virtual void Register()
        {
            List.OnSelectedEvent += List_OnSelectedEvent;
            List.OnDeselectedEvent += List_OnDeselectedEvent;
            List.OnSubmitEvent += List_OnSubmitEvent;
            List.OnMoveEvent += List_OnMoveEvent;
            List.OnClearItemEvent += List_OnClearItemEvent;
        }

        private void List_OnClearItemEvent(NavigationItem obj)
        {
            OnItemUnbindData(obj);
        }

        private void List_OnMoveEvent(float h, float v, NavigationItem obj)
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = GameWorld.FindEntity<NavigationListEntity>(guid);
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
                NavigationListEntity e = GameWorld.FindEntity<NavigationListEntity>(guid);
                e?.OnSubmit(item);
            }
        }

        private void List_OnDeselectedEvent(NavigationItem obj)
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = GameWorld.FindEntity<NavigationListEntity>(guid);
                e?.OnDeselect(item);
            }
        }

        private void List_OnSelectedEvent(NavigationItem obj)
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = GameWorld.FindEntity<NavigationListEntity>(guid);
                e?.OnSelect(item);
            }
        }

        protected void OnItemBindData(NavigationItem obj) 
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = GameWorld.FindEntity<NavigationListEntity>(guid);
                e?.OnBindData(item);
            }
        }

        protected void OnItemUnbindData(NavigationItem obj)
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = GameWorld.FindEntity<NavigationListEntity>(guid);
                e?.OnUnbindData(item);
            }
        }

        protected void OnItemRefresh(NavigationItem obj) 
        {
            if (obj != null && obj is GameNavigationItem item)
            {
                NavigationListEntity e = GameWorld.FindEntity<NavigationListEntity>(guid);
                e?.OnRefresh(item);
            }
        }

        public abstract void UpdateData(object[] data);

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
