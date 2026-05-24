using GameFramework.Core;
using GameFramework.Utility.GameDefine;
using GameFramework.View.UI;

namespace GameFramework.View
{
    [GameSystem]
    public class UISystem : IGameSystem, IInit
    {
        private UIControllerManager controllerManager;
        private UIEntityManager entityManager;
        private UINavigationManager navigationManager;

        public void Init()
        {
            controllerManager = new UIControllerManager();
            controllerManager.Init();

            entityManager = new UIEntityManager();
            entityManager.Init();

            navigationManager = new UINavigationManager();
            navigationManager.Init();
        }

        /// <summary>
        /// 进入导航模式。导航到一个导航组
        /// </summary>
        /// <param name="id"></param>
        /// <param name="defaultIndexs"></param>
        public void Navigate(NavigationDefine id, int[] defaultIndexs = null)
        {
            navigationManager.Navigate(id, defaultIndexs);
        }

        /// <summary>
        /// 退出导航模式
        /// </summary>
        public void CloseNavigate()
        {
            navigationManager.CloseNavigate();
        }

        public void ShowPanel(PanelDefine id, object content = null)
        {
            entityManager.ShowPanel(id, content);
        }

        public void HidePanel(PanelDefine id)
        {
            entityManager.HidePanel(id);
        }

        public PanelEntity GetPanelEntity(PanelDefine id)
        {
            return entityManager.GetPanelEntity(id);
        }

        public NavigationListEntity GetNavigationListEntity(NavigationDefine id)
        {
            return entityManager.GetNavigationListEntity(id);
        }

        public IPanelController GetPanelController(PanelDefine id)
        {
            return controllerManager.GetPanelController(id);
        }

        public INavigationController GetNavigationController(NavigationDefine id)
        {
            return controllerManager.GetNavigationController(id);
        }

        public PanelDefine GetPanelDefine(NavigationDefine id)
        {
            return controllerManager.GetPanelDefine(id);
        }

        public NavigationDefine[] GetNavigationDefines(PanelDefine id)
        {
            return controllerManager.GetNavigationDefines(id);
        }
    }
}