using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupEntity
    {
        private NavigationGroupProxy proxy;

        private Dictionary<NavigationListDefine, NavigationListEntity> lists;

        private NavigationGroupDefine define;
        private bool isLoaded;

        public void Init(NavigationGroupDefine define) 
        {            
            this.define = define;
            this.isLoaded = false;
        }

        public NavigationListEntity GetNavigationListEntity(NavigationListDefine define) 
        {
            if (lists == null || lists.Count == 0) 
            {
                return null;
            }

            if (!lists.ContainsKey(define)) 
            {
                return null;
            }
            
            return lists[define];
        }

        public void Show() 
        {
            if (!isLoaded) 
            {
                Load();
            }

            proxy?.Show();
        }

        public void Hide() 
        {
            proxy?.Hide();
        }

        public void Refocus() 
        {
            proxy?.Refocus();
        }

        public void OutFocus() 
        {
            proxy?.OutFocus();
        }

        public void OnDestroy()
        {
            proxy?.OnDestroy();
            proxy = null;
        }

        private void Load() 
        {
            proxy = NavigationProxyManager.GetNavigationGroupProxy(define);
            NavigationGroupView group = proxy.LoadGroup(define);

            lists = new Dictionary<NavigationListDefine, NavigationListEntity>();
            NavigationListView[] children = group.Children;
            if (children != null && children.Length > 0)
            {
                foreach (var v in children)
                {
                    NavigationListEntity e = new NavigationListEntity();
                    lists[v.Define] = e;
                    e.Init(v);
                }
            }

            //list第一次Awake和OnEnable在Group实例化的时候就已经执行了，那时proxy还未初始化，会执行失败
            //在这里手动调用初次Awake，OnEnable；后续的生命周期和unity一致
            foreach (var e in lists.Values)
            {
                e.Awake();
            }
            foreach (var e in lists.Values)
            {
                e.OnEnable();
            }
            isLoaded = true;
        }
    }
}
