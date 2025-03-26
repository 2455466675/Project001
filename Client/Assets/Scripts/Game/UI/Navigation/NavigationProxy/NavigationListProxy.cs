namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationListProxy
    {
        #region Unity生命周期
        public virtual void Awake(NavigationListView list) 
        {            
        }
        public virtual void OnEnable(NavigationListView list) 
        {
        }
        public virtual void OnDisable(NavigationListView list) 
        {        
        }
        #endregion

        #region 导航行为
        public virtual void Move(NavigationListView list, float h, float v)
        {
            if (list == null) 
            {
                return;
            }
            list.Move(h, v);
        }
        public virtual void Submit(NavigationListView list)
        {
            if (list == null)
            {
                return;
            }
            list.Submit();
        }
        public virtual bool InFocus(NavigationListView list, bool isRefocus, int[] indexs = null)
        {
            if (list == null)
            {
                return false;
            }
            return list.InFocus(isRefocus, indexs);
        }
        public virtual void OutFocus(NavigationListView list)
        {
            if (list == null)
            {
                return;
            }
            list.OutFocus();
        }
        public virtual void Exit(NavigationListView list) 
        {
            if (list == null)
            {
                return;
            }
            list.Exit();
        }
        public virtual bool IsLocked() 
        {
            return false;
        }
        #endregion

        #region 列表项事件

        public virtual void OnBindData(NavigationListView list, GameNavigationItem item) 
        {
        }
        public virtual void OnUnbindData(NavigationListView list, GameNavigationItem item)
        {
        }
        public virtual void OnRefresh(NavigationListView list, GameNavigationItem item) 
        {
        }
        public virtual void OnSelect(NavigationListView list, GameNavigationItem item)
        {
        }
        public virtual void OnDeselect(NavigationListView list, GameNavigationItem item)
        {
        }
        public virtual void OnSubmit(NavigationListView list, GameNavigationItem item)
        {
        }
        public virtual void OnMoveUp(NavigationListView list, GameNavigationItem item)
        {
        }
        public virtual void OnMoveDown(NavigationListView list, GameNavigationItem item)
        {
        }
        public virtual void OnMoveLeft(NavigationListView list, GameNavigationItem item)
        {
        }
        public virtual void OnMoveRight(NavigationListView list, GameNavigationItem item)
        {
        }
        #endregion
    }
}
