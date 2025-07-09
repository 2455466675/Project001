using Navigation;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationListProxy
    {
        private NavigationListView m_view;
        protected NavigationListView ListView => m_view;

        private NavigationGroupProxy m_parent;
        protected NavigationGroupProxy Parent => m_parent;

        public void Init(NavigationListView view, NavigationGroupProxy parent) 
        {
            this.m_view = view;
            this.m_parent = parent;
        }

        public void UpdateData(INavigationItemData[] datas) 
        {
            datas ??= new INavigationItemData[0];
            datas = FilterData(datas);
            ListView.UpdateData(datas);
        }

        protected virtual INavigationItemData[] FilterData(INavigationItemData[] datas) 
        {
            return datas;
        }

        #region Unity生命周期
        public virtual void Awake() 
        {            
        }
        /// <summary>
        /// 等同于Unity的OnEnable();
        /// 在这里注册数据
        /// </summary>
        public virtual void OnEnable() 
        {
        }
        /// <summary>
        /// 等同于Unity的OnDisable();
        /// 在这里取消注册
        /// </summary>
        public virtual void OnDisable() 
        {        
        }
        public void OnDestroy() 
        {
            m_view = null;
            m_parent = null;
        }
        #endregion

        #region 导航行为
        public virtual void Move(float h, float v)
        {
            if (ListView == null) 
            {
                return;
            }
            ListView.Move(h, v);
        }
        public virtual void Submit()
        {
            if (ListView == null)
            {
                return;
            }
            ListView.Submit();
        }
        public virtual bool InFocus(bool isRefocus, int[] indexs = null)
        {
            if (ListView == null)
            {
                return false;
            }
            return ListView.InFocus(isRefocus, indexs);
        }
        public virtual void OutFocus()
        {
            if (ListView == null)
            {
                return;
            }
            ListView.OutFocus();
        }
        public virtual void Exit() 
        {
            if (ListView == null)
            {
                return;
            }
            ListView.Exit();
        }
        public virtual bool IsLocked() 
        {
            return false;
        }
        #endregion

        #region 列表项事件
        /// <summary>
        /// 列表项绑定了数据时
        /// 在这里注册事件相关
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnBindData(GameNavigationItem item) 
        {
        }
        /// <summary>
        /// 列表项取消了数据绑定
        /// 在这里取消事件注册
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnUnbindData(GameNavigationItem item)
        {
        }
        /// <summary>
        /// 刷新列表项
        /// 在这里更新列表项的view
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnRefresh(GameNavigationItem item) 
        {
        }
        /// <summary>
        /// 列表项被选中时
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnSelect(GameNavigationItem item)
        {
        }
        /// <summary>
        /// 列表项取消选中时
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnDeselect(GameNavigationItem item)
        {
        }
        /// <summary>
        /// 列表项“确定”
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnSubmit(GameNavigationItem item)
        {
        }
        /// <summary>
        /// 当选中此列表项时，输入了向上移动的指令
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnMoveUp(GameNavigationItem item)
        {
        }
        /// <summary>
        /// 当选中此列表项时，输入了向下移动的指令
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnMoveDown(GameNavigationItem item)
        {
        }
        /// <summary>
        /// 当选中此列表项时，输入了向左移动的指令
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnMoveLeft(GameNavigationItem item)
        {
        }
        /// <summary>
        /// 当选中此列表项时，输入了向右移动的指令
        /// </summary>
        /// <param name="item"></param>
        public virtual void OnMoveRight(GameNavigationItem item)
        {
        }
        #endregion
    }
}
