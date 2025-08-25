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

        private PanelController m_Controller;

        public void SetId(PanelDefine id)
        {
            m_Id = id;
        }

        public void Show(PanelController controller, object content)
        {
            var cfg = Game.GetModule<ConfigManager>().Find<PanelCfg>((int)m_Id);
            var go = Game.GetModule<ResourceManager>().LoadAndInstantiate(cfg.Path, null);
            var panel = go.GetComponent<UIPanel>();

            controller.Show(panel, content);
            m_Controller = controller;
        }

        public void Hide()
        {
            m_Controller.Hide();
            m_Controller = null;
        }
    }
}