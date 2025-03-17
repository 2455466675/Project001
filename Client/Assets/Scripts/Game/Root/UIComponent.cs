using Config;
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

        public void Navigate(NavigationListDefine list_ID, ModuleType moduleType = ModuleType.Panel) 
        {
            controller.Navigate(list_ID, moduleType);
        }

        public NavigationGroupDefine ListDefineToGroupDefine(NavigationListDefine listDefine)
        {
            return navigationManager.ListDefineToGroupDefine(listDefine);
        }

        public void ShowGroup(NavigationGroupDefine define) 
        {
            navigationManager.ShowGroup(define);
        }

        public void HideGroup(NavigationGroupDefine define)
        {
            navigationManager.HideGroup(define);
        }

        public void RefocusGroup(NavigationGroupDefine define)
        {
            navigationManager.RefocusGroup(define);
        }

        public void OutFocusGroup(NavigationGroupDefine define)
        {
            navigationManager.OutFocusGroup(define);
        }

        public void Move(NavigationListDefine define, float h, float v)
        {
            navigationManager.Move(define, h, v);
        }

        public void Submit(NavigationListDefine define)
        {
            navigationManager.Submit(define);
        }

        public bool InFocus(NavigationListDefine define, bool isRefocus, int[] indexs = null)
        {
            return navigationManager.InFocus(define, isRefocus, indexs);
        }

        public void OutFocus(NavigationListDefine define)
        {
            navigationManager.OutFocus(define);
        }

        public void Exit(NavigationListDefine define)
        {
            navigationManager.Exit(define);
        }
    }
}
