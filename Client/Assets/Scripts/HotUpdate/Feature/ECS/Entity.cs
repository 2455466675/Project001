using System;
using System.Collections.Generic;

namespace GameFramework.Featrue 
{
    public sealed class Entity : IAddComponent, IGetComponent
    {
        public int Eid { get; private set; }
        private List<Component> m_Components;
        private Action<Component> m_RegisterComponent;
        private Action<Component> m_UnregisterComponent;

        internal Entity(int eid, Action<Component> registerComponent, Action<Component> unregisterComponent) 
        {
            Eid = eid;
            m_Components = new List<Component>();
            this.m_RegisterComponent = registerComponent;
            this.m_UnregisterComponent = unregisterComponent;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = GetComponent<T>();
            if (component == null) 
            {
                component = new();
                component.Init(this);
                m_Components.Add(component);
                m_RegisterComponent?.Invoke(component);
            }
            
            return component;
        }

        public T GetComponent<T>() where T : Component
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                Component component = m_Components[i];
                if (component is T)
                {
                    return component as T;
                }
            }

            return default;
        }

        internal T Internal_AddComponent<T>() where T : Component, new()
        {
            T component = GetComponent<T>();
            if (component == null)
            {
                component = new();
                m_Components.Add(component);
            }
            return component;
        }

        internal void Destroy() 
        {
            foreach (var component in m_Components)
            {
                m_UnregisterComponent?.Invoke(component);
                component.Destroy();             
            }
            m_Components.Clear();
            m_Components = null;
            m_RegisterComponent = null;
            m_UnregisterComponent = null;
        }
    }
}