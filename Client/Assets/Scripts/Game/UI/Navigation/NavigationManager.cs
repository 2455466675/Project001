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
            if (!groups.ContainsKey(define))
            {
                return;
            }
            groups[define].Hide();
        }

        public void RefocusGroup(NavigationGroupDefine define)
        {
            if (!groups.ContainsKey(define))
            {
                return;
            }
            groups[define].Refocus();
        }

        public void OutFocusGroup(NavigationGroupDefine define)
        {
            if (!groups.ContainsKey(define))
            {
                return;
            }
            groups[define].OutFocus();
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

        public void Move(NavigationListDefine define, float h, float v) 
        {
            NavigationListEntity listEntity = GetNavigationListEntity(define);
            listEntity?.Move(h, v);
        }

        public void Submit(NavigationListDefine define)
        {
            NavigationListEntity listEntity = GetNavigationListEntity(define);
            listEntity?.Submit();
        }

        public bool InFocus(NavigationListDefine define, bool isRefocus, int[] indexs = null)
        {
            NavigationListEntity listEntity = GetNavigationListEntity(define);
            if (listEntity == null) 
            {
                return false;
            }
            return listEntity.InFocus(isRefocus, indexs);            
        }

        public void OutFocus(NavigationListDefine define)
        {
            NavigationListEntity listEntity = GetNavigationListEntity(define);
            listEntity?.OutFocus();
        }

        public void Exit(NavigationListDefine define)
        {
            NavigationListEntity listEntity = GetNavigationListEntity(define);
            listEntity?.Exit();
        }

        #endregion
    }
}