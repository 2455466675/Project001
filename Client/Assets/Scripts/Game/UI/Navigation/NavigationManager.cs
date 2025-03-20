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

            AddComponent<NavigationProxy>();
        }

        public void LoadMap(string path) 
        {
            navigationMap = GameWorld.Root.GetComponent<ResourceComponent>().LoadFormRes<NavigationMap>(path);
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
                return null;
            }
        }

        public void ShowGroup(NavigationGroupDefine define) 
        {
            if (groups.ContainsKey(define)) 
            {
                groups[define].Show();
            }
            else
            {
                NavigationGroupEntity groupEntity = CreateChild<NavigationGroupEntity>();
                groupEntity.Init(define);
                groupEntity.Show();
                groups[define] = groupEntity;
            }
        }

        public void HideGroup(NavigationGroupDefine define)
        {
            var entity = GetNavigationGroupEntity(define);
            entity?.Hide();            
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