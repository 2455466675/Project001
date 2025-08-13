using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.UI
{
    public abstract class PanelController
    {
        private UIPanel m_Panel;

        private NavigationController[] m_SubControllers;

        private object m_Content;

        public void SetNavigationControllers(NavigationController[] controllers) 
        {
            m_SubControllers = controllers;
        }

        public void Show(UIPanel panel, object content)
        {
            m_Panel = panel;
            m_Content = content;

            SubShow();
            OnShow();
        }

        public void Hide()
        {
            SubHide();
            OnHide();

            m_Panel = null;
            m_Content = null;
        }

        protected T GetWidget<T>() where T : UIWidget
        {
            return m_Panel.GetWidget<T>();
        }

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }

        protected T GetContent<T>() where T : class
        {
            if (m_Content == null) 
            {
                return default;
            }
            else
            {
                return m_Content as T;
            }
        }

        private void SubShow() 
        {
            var navigationViews = m_Panel.GetNavigationViews();
            if (navigationViews == null || navigationViews.Length != m_SubControllers.Length)
            {
                return;
            }

            for (int i = 0; i < m_SubControllers.Length; i++)
            {
                NavigationController controller = m_SubControllers[i];
                NavigationView view = navigationViews[i];
                controller.Show(view);
            }
        }

        private void SubHide() 
        {
            for (int i = 0; i < m_SubControllers.Length; i++)
            {
                NavigationController controller = m_SubControllers[i];
                controller.Hide();
            }
        }
    }
}