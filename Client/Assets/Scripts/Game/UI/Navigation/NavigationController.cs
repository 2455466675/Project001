using System.Collections.Generic;

namespace Game.UI
{
    public class NavigationController
    {
        private readonly NavigationMap navigationMap;
        private readonly Dictionary<NavigationGroupDefine, NavigationGroupEntity> groups;

        public NavigationController(string map) 
        {
            navigationMap = Game.Resource.LoadFormRes<NavigationMap>(map);
            groups = new Dictionary<NavigationGroupDefine, NavigationGroupEntity>();

            NavigationProxyManager.Init();
        }

        public NavigationGroupDefine ListDefineToGroupDefine(NavigationListDefine listDefine)
        {
            return navigationMap.GetGroupDefine(listDefine);
        }

        #region NavigationGroup

        public NavigationGroupEntity GetNavigationGroupEntity(NavigationGroupDefine define)
        {
            if (groups.ContainsKey(define))
            {
                return groups[define];
            }
            else
            {
                NavigationGroupEntity groupEntity = new NavigationGroupEntity();
                groupEntity.Init(define);
                groups[define] = groupEntity;
                return groupEntity;
            }
        }

        #endregion

        #region NavigationList

        public NavigationListEntity GetNavigationListEntity(NavigationListDefine listDefine)
        {
            NavigationGroupDefine groupDefine = ListDefineToGroupDefine(listDefine);
            NavigationGroupEntity groupEntity = GetNavigationGroupEntity(groupDefine);
            if (groupEntity == null)
            {
                return null;
            }
            else
            {
                return groupEntity.GetNavigationListEntity(listDefine);
            }
        }
        #endregion
    }
}