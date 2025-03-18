using Navigation;
using System.Collections;
using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationGroupEntity : ECS.Entity
    {
        private NavigationGroupProxy proxy;
        private NavigationGroup group;

        private Dictionary<NavigationListDefine, NavigationListEntity> lists;

        public void Init(NavigationGroupDefine define) 
        {
            proxy = new NavigationGroupProxy();
            NavigationGroupView view = proxy.LoadGroup(define);

            group = view.Group;

            lists = new Dictionary<NavigationListDefine, NavigationListEntity>();
            NavigationListView[] children = view.Children;
            if (children != null && children.Length > 0 ) 
            {
                foreach (var v in children)
                {
                    NavigationListEntity e = CreateChild<NavigationListEntity>();
                    e.Init(v.define, v.List);
                    lists[v.define] = e;
                }
            }
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
