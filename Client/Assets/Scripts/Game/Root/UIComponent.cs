using Game.UI;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class UIComponent : ECS.Entity
    {
        public UIRoot Root { get; private set; }

        private InputController controller;
        private NavigationManager navigationManager;

        public void Init(GameInitConfig config) 
        {
            var go = GameWorld.Root.GetComponent<ResourceComponent>().LoadAndInstantiate(config.UIRootPath, null);
            Root = go.GetComponent<UIRoot>();

            controller = new InputController();
            navigationManager = AddComponent<NavigationManager>();
            navigationManager.LoadMap(config.NavigationMap);            
        }

        public void InputAction(ActionContext context) 
        {
            controller.InputAction(context);
        }

        public void Navigate(NavigationListDefine list_ID, ModuleType moduleType = ModuleType.Undefined, int[] navigateIndexs = null) 
        {
            controller.Navigate(list_ID, moduleType, navigateIndexs);
        }

        public void Close() 
        {
            ActionContext context = new()
            {
                InputType = Input.InputType.Esc,
            };
            InputAction(context);
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
