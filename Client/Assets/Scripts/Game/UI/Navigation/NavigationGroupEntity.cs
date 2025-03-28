using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupEntity : ECS.Entity
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
            if (lists.ContainsKey(define)) 
            {
                return lists[define];
            }
            else
            {
                return null;
            }
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

        protected override void OnDestroy()
        {
            proxy?.OnDestroy();
            proxy = null;
        }

        private void Load() 
        {
            proxy = Parent.GetComponent<NavigationProxy>().GetNavigationGroupProxy(define);
            NavigationGroupView group = proxy.LoadGroup(define);

            lists = new Dictionary<NavigationListDefine, NavigationListEntity>();
            NavigationListView[] children = group.Children;
            if (children != null && children.Length > 0)
            {
                foreach (var v in children)
                {
                    NavigationListEntity e = CreateChild<NavigationListEntity>();
                    e.Init(v);
                    lists[v.Define] = e;
                }
            }

            isLoaded = true;
        }
    }
}
