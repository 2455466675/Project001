using Navigation;
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

        public void Init(NavigationGroupDefine define) 
        {            
            proxy = Parent.GetComponent<NavigationProxy>().GetNavigationGroupProxy(define);
            NavigationGroupView group = proxy.LoadGroup(define);

            lists = new Dictionary<NavigationListDefine, NavigationListEntity>();
            NavigationListView[] children = group.Children;
            if (children != null && children.Length > 0 ) 
            {
                foreach (var v in children)
                {
                    NavigationListEntity e = CreateChild<NavigationListEntity>();
                    lists[v.Define] = e;
                    e.Init(v);
                }
            }

            this.group = group;
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
            proxy?.InFocus(group, false);
        }

        public void Hide() 
        {
            proxy?.Exit(group);
        }

        public void Refocus() 
        {
            proxy?.InFocus(group, true);
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
    }
}
