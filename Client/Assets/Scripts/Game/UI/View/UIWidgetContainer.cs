using System;
using UnityEngine;

namespace GameFramework.UI 
{
    public abstract class UIWidgetContainer : UIView
    {
        [Serializable]
        private class WidgetItem 
        {
            public string key;
            public UIWidget widget;
        }

        [SerializeField]
        private WidgetItem[] m_Widgets;

        public T GetWidget<T>() where T : UIWidget 
        {
            if (m_Widgets == null || m_Widgets.Length == 0) 
            {
                return default;
            }

            Type type = typeof(T);
            for (int i = 0; i < m_Widgets.Length; i++)
            {
                WidgetItem item = m_Widgets[i];
                if (item.widget.GetType() == type) 
                {
                    return item.widget as T;
                }
            }
            return default;
        }

        public T GetWidget<T>(string key) where T : UIWidget
        {
            if (m_Widgets == null || m_Widgets.Length == 0)
            {
                return default;
            }

            Type type = typeof(T);
            for (int i = 0; i < m_Widgets.Length; i++)
            {
                WidgetItem item = m_Widgets[i];
                if (item.key == key && item.widget.GetType() == type)
                {
                    return item.widget as T;
                }
            }
            return default;
        }
    }
}