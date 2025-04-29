using Game.UI.Input;

namespace Game.UI
{
    public class GameUI
    {
        public UIRoot Root { get; private set; }

        private InputController inputController;
        private NavigationController navigationController;

        public void Init()
        {
            var go = Game.Resource.LoadAndInstantiate(Game.Config.Formula.UIRootPath, Game.Root.transform);
            Root = go.GetComponent<UIRoot>();

            inputController = new InputController();
            navigationController = new NavigationController(Game.Config.Formula.NavigationMap);
        }

        public void FixedUpdate(float dt) 
        {
            inputController.FixedUpdate(dt);
        }

        public void Navigate(NavigationListDefine list_ID, ModuleType moduleType = ModuleType.Panel, int[] navigateIndexs = null)
        {
            inputController.Navigate(list_ID, moduleType, navigateIndexs);
        }

        public void Back()
        {
            inputController.Back();
        }

        public void Close()
        {
            inputController.Close();
        }
        public NavigationGroupDefine ListDefineToGroupDefine(NavigationListDefine listDefine)
        {
            return navigationController.ListDefineToGroupDefine(listDefine);
        }

        public NavigationGroupEntity GetNavigationGroupEntity(NavigationGroupDefine define)
        {
            return navigationController.GetNavigationGroupEntity(define);
        }

        public NavigationListEntity GetNavigationListEntity(NavigationListDefine listDefine)
        {
            return navigationController.GetNavigationListEntity(listDefine);
        }
    }
}