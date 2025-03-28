namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationListProxy
    {
        protected NavigationListView listView;

        public void Init(NavigationListView listView) 
        {
            this.listView = listView;
        }

        #region Unity生命周期
        public virtual void Awake() 
        {            
        }
        public virtual void OnEnable() 
        {
        }
        public virtual void OnDisable() 
        {        
        }
        public void OnDestroy() 
        {
            listView = null;
        }
        #endregion

        #region 导航行为
        public virtual void Move(float h, float v)
        {
            if (listView == null) 
            {
                return;
            }
            listView.Move(h, v);
        }
        public virtual void Submit()
        {
            if (listView == null)
            {
                return;
            }
            listView.Submit();
        }
        public virtual bool InFocus(bool isRefocus, int[] indexs = null)
        {
            if (listView == null)
            {
                return false;
            }
            return listView.InFocus(isRefocus, indexs);
        }
        public virtual void OutFocus()
        {
            if (listView == null)
            {
                return;
            }
            listView.OutFocus();
        }
        public virtual void Exit() 
        {
            if (listView == null)
            {
                return;
            }
            listView.Exit();
        }
        public virtual bool IsLocked() 
        {
            return false;
        }
        #endregion

        #region 列表项事件

        public virtual void OnBindData(GameNavigationItem item) 
        {
        }
        public virtual void OnUnbindData(GameNavigationItem item)
        {
        }
        public virtual void OnRefresh(GameNavigationItem item) 
        {
        }
        public virtual void OnSelect(GameNavigationItem item)
        {
        }
        public virtual void OnDeselect(GameNavigationItem item)
        {
        }
        public virtual void OnSubmit(GameNavigationItem item)
        {
        }
        public virtual void OnMoveUp(GameNavigationItem item)
        {
        }
        public virtual void OnMoveDown(GameNavigationItem item)
        {
        }
        public virtual void OnMoveLeft(GameNavigationItem item)
        {
        }
        public virtual void OnMoveRight(GameNavigationItem item)
        {
        }
        #endregion
    }
}
