using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupEntity : ECS.Entity
    {
        private NavigationGroupProxy proxy;
        private NavigationGroupView group;

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

            proxy?.Show(group);
        }

        public void Hide() 
        {
            proxy?.Hide(group);
        }

        public void Refocus() 
        {
            proxy?.Refocus(group);
        }

        public void OutFocus() 
        {
            proxy?.OutFocus(group);
        }

        protected override void OnDestroy()
        {
            UnityEngine.Object.Destroy(group);
            group = null;
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

            this.group = group;
            isLoaded = true;
        }
    }
}
