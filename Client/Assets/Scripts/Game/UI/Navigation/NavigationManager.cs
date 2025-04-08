using ECS;
using System.Collections.Generic;

namespace Game.UI
{
    public class NavigationManager : Entity, IAwake
    {
        private NavigationMap navigationMap;

        private Dictionary<NavigationGroupDefine, NavigationGroupEntity> groups;

        public void Awake()
        {
            groups = new Dictionary<NavigationGroupDefine, NavigationGroupEntity>();

            //AddComponent<NavigationProxy>();
        }

        public void LoadMap(string path) 
        {
            //navigationMap = GameWorld.Root.GetComponent<ResourceComponent>().LoadFormRes<NavigationMap>(path);
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