using Config;
using GameFramework.Core;

namespace GameFramework.UI 
{
    public class PanelComponent : Featrue.Component
    {
        private UIPanel m_UIPanel;
        private PanelController m_Controller;

        protected override void OnDestroy()
        {
            GoHelper.Destroy(m_UIPanel.gameObject);
            m_UIPanel = null;
            m_Controller = null;
        }

        public void Init(PanelDefine id, PanelController controller) 
        {
            //var cfg = Game.GetModule<ConfigManager>().Find<PanelCfg>((int)id);
            //var parent = GameRoot.Instance.UIRoot.GetGroupContainer(cfg.GroupType);
            //var go = Game.GetModule<ResourceManager>().LoadAndInstantiate(cfg.Path, parent);
            //var panel = go.GetComponent<UIPanel>();

            //m_UIPanel = panel;
            //m_Controller = controller;
        }

        public NavigationView[] GetNavigationViews()
        {
            return m_UIPanel.GetNavigationViews();
        }

        public void Show(object content)
        {
            m_Controller.Show(m_UIPanel, content);
        }

        public void Hide()
        {
            m_Controller.Hide();
        }

        public void Refocus()
        {
            m_Controller.Refocus();
        }

        public void OutFocus()
        {
            m_Controller.OutFocus();
        }

        public bool CheckLocked()
        {
            return m_Controller.IsLocked;
        }
    }
}