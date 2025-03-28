namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationListEntity : ECS.Entity
    {
        public bool IsLocked => proxy != null && proxy.IsLocked();
        private NavigationListProxy proxy;

        public void Init(NavigationListView list) 
        {
            list.Bind(Guid);
            proxy = Parent.Parent.GetComponent<NavigationProxy>().GetNavigationListProxy(list.Define);
            proxy.Init(list);
            Awake();
            OnEnable();
        }

        public void Awake() 
        {
            proxy?.Awake();
        }

        public void OnEnable() 
        {
            proxy?.OnEnable();
        }

        public void OnDisable() 
        {
            proxy?.OnDisable();
        }

        public void Move(float h, float v)
        {
            proxy?.Move(h, v);
        }

        public void Submit()
        {
            proxy?.Submit();
        }

        public bool InFocus(bool isRefocus, int[] indexs = null)
        {
            if (proxy == null) 
            {
                return false;
            }
            else
            {
                return proxy.InFocus(isRefocus, indexs);                
            }
        }

        public void OutFocus()
        {
            proxy?.OutFocus();
        }

        public void Exit()
        {
            proxy?.Exit();
        }

        public void OnBindData(GameNavigationItem item)
        {
            proxy?.OnBindData(item);
        }

        public void OnUnbindData(GameNavigationItem item)
        {
            proxy?.OnUnbindData(item);
        }

        public void OnRefresh(GameNavigationItem item) 
        {
            proxy?.OnRefresh(item);
        }

        public void OnSelect(GameNavigationItem item) 
        {
            proxy?.OnSelect(item);    
        }

        public void OnDeselect(GameNavigationItem item) 
        {
            proxy?.OnDeselect(item);
        }

        public void OnSubmit(GameNavigationItem item) 
        {
            proxy?.OnSubmit(item);
        }

        public void OnMoveUp(GameNavigationItem item)
        {
            proxy?.OnMoveUp(item);
        }
        public void OnMoveDown(GameNavigationItem item)
        {
            proxy?.OnMoveDown(item);
        }
        public void OnMoveLeft(GameNavigationItem item)
        {
            proxy?.OnMoveLeft(item);
        }
        public void OnMoveRight(GameNavigationItem item)
        {
            proxy?.OnMoveRight(item);
        }

        protected override void OnDestroy()
        {
            proxy?.OnDestroy();
            proxy = null;
        }
    }
}
