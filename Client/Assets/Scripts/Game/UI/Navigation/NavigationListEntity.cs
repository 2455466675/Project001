namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationListEntity : ECS.Entity
    {
        public NavigationListDefine Define => list.Define;
        public bool IsLocked => proxy != null && proxy.IsLocked();

        private NavigationListView list;
        private NavigationListProxy proxy;

        public void Init(NavigationListView list) 
        {
            list.Bind(Guid);
            this.list = list;
            proxy = Parent.Parent.GetComponent<NavigationProxy>().GetNavigationListProxy(Define);

            Awake();
            OnEnable();
        }

        public void Awake() 
        {
            proxy?.Awake(list);
        }

        public void OnEnable() 
        {
            proxy?.OnEnable(list);
        }

        public void OnDisable() 
        {
            proxy?.OnDisable(list);
        }

        public void Move(float h, float v)
        {
            proxy?.Move(list, h, v);
        }

        public void Submit()
        {
            proxy?.Submit(list);
        }

        public bool InFocus(bool isRefocus, int[] indexs = null)
        {
            if (proxy == null) 
            {
                return false;
            }
            else
            {
                return proxy.InFocus(list, isRefocus, indexs);                
            }
        }

        public void OutFocus()
        {
            proxy?.OutFocus(list);
        }

        public void Exit()
        {
            proxy?.Exit(list);
        }

        public void OnSelect(GameNavigationItem item) 
        {
            proxy?.OnSelect(list, item);    
        }

        public void OnDeselect(GameNavigationItem item) 
        {
            proxy?.OnDeselect(list, item);
        }

        public void OnSubmit(GameNavigationItem item) 
        {
            proxy?.OnSubmit(list, item);
        }

        public void OnMoveUp(GameNavigationItem item)
        {
            proxy?.OnMoveUp(list, item);
        }
        public void OnMoveDown(GameNavigationItem item)
        {
            proxy?.OnMoveDown(list, item);
        }
        public void OnMoveLeft(GameNavigationItem item)
        {
            proxy?.OnMoveLeft(list, item);
        }
        public void OnMoveRight(GameNavigationItem item)
        {
            proxy?.OnMoveRight(list, item);
        }

        protected override void OnDestroy()
        {
            list = null;
            proxy = null;
        }
    }
}
