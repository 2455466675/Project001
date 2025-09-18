using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UI 
{
    public class NavigationListComponent : Featrue.Component
    {
        private NavigationView m_View;
        private NavigationController m_Controller;

        public void Init(NavigationView view, NavigationController controller)
        {
            m_View = view;
            m_Controller = controller;
        }

        public void Show() 
        {
            m_Controller?.Show(m_View);
        }

        public void Hide() 
        {
            m_Controller?.Hide();
        }

        public void Move(float h, float v) 
        {
            m_Controller?.Move(h, v);
        }

        public void Submit() 
        {
            m_Controller?.Submit();
        }

        public bool InFocus(bool isRefocus, int[] indexs = null) 
        {
            if (m_Controller == null) return false;
            return m_Controller.InFocus(isRefocus, indexs);
        }

        public void OutFocus() 
        {
            m_Controller?.OutFocus();
        }

        public void Exit() 
        {
            m_Controller?.Exit();
        }

        public bool CheckLocked()
        {
            if (m_Controller == null) return false;
            return m_Controller.IsLocked;
        }
    }
}