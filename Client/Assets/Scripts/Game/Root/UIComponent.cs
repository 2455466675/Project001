using Game.UI;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class UIComponent : ECS.Entity
    {
        public UIRoot Root { get; private set; }

        private NavigationManager navigationManager;

        public void Init(GameInitConfig config) 
        {
            //var go = GameWorld.Root.GetComponent<ResourceComponent>().LoadAndInstantiate(config.UIRootPath, GameWorld.GameWorldObject.transform);
            //Root = go.GetComponent<UIRoot>();


            navigationManager = AddComponent<NavigationManager>();
            navigationManager.LoadMap(config.NavigationMap);            
        }

        //public void InputAction(ActionContext context) 
        //{

        //}

        //public void Navigate(NavigationListDefine list_ID, ModuleType moduleType = ModuleType.Undefined, int[] navigateIndexs = null) 
        //{

        //}

        public void Close() 
        {

        }

        public NavigationGroupDefine ListDefineToGroupDefine(NavigationListDefine listDefine)
        {
            return navigationManager.ListDefineToGroupDefine(listDefine);
        }

        public NavigationGroupEntity GetNavigationGroupEntity(NavigationGroupDefine define) 
        {
            return navigationManager.GetNavigationGroupEntity(define);
        }

        public NavigationListEntity GetNavigationListEntity(NavigationListDefine listDefine) 
        {
            return navigationManager.GetNavigationListEntity(listDefine);
        }
    }
}
