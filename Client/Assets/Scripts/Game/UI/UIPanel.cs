using Config;
using GameFramework.Core;
using GameFramework.Featrue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameFramework.UI 
{
    public class UIPanel : UIWidgetContainer
    {
        [SerializeField]
        private NavigationView[] navigation;

        public NavigationView[] GetNavigationViews()
        {
            return navigation;
        }
    }

    public class PanelComponent : Featrue.Component
    {
        private PanelDefine m_Id;
        private UIPanel m_UIPanel;
        private PanelController m_Controller;

        public void SetId(PanelDefine id)
        {
            m_Id = id;
        }

        public void Load(PanelController controller) 
        {
            var cfg = Game.GetModule<ConfigManager>().Find<PanelCfg>((int)m_Id);
            var parent = GameRoot.Instance.UIRoot.GetGroupContainer(cfg.GroupType);
            var go = Game.GetModule<ResourceManager>().LoadAndInstantiate(cfg.Path, parent);
            m_UIPanel = go.GetComponent<UIPanel>();
            m_Controller = controller;
        }

        public void Show(object content)
        {
            m_Controller.Show(m_UIPanel, content);
        }

        public void Hide()
        {
            m_Controller.Hide();
        }
    }
}