using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.View.UI
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
                if (item.widget is T result) 
                {
                    return result;
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

            for (int i = 0; i < m_Widgets.Length; i++)
            {
                WidgetItem item = m_Widgets[i];
                if (item.key == key && item.widget is T result)
                {
                    return result;
                }
            }
            return default;
        }

#if UNITY_EDITOR

        protected virtual void InitEditor() 
        {
            void Fun(Transform tf, List<UIWidget> widgets)
            {
                var r = tf.GetComponents<UIWidget>();
                if (r != null && r.Length > 0)
                {
                    widgets.AddRange(r);
                }

                int childCount = tf.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    var child = tf.GetChild(i);
                    var container = child.GetComponent<UIWidgetContainer>();
                    if (container != null)
                    {
                        container.InitEditor();
                        continue;
                    }
                    else
                    {
                        Fun(child, widgets);
                    }
                }
            }

            List<UIWidget> widgets = new List<UIWidget>();
            Fun(transform, widgets);

            m_Widgets = new WidgetItem[widgets.Count];
            for (int i = 0; i < widgets.Count; i++)
            {
                var widget = widgets[i];
                m_Widgets[i] = new WidgetItem() { key = widget.gameObject.name, widget = widget };
            }
        }
#endif
    }
}