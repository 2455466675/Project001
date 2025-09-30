using System.Collections.Generic;

namespace GameFramework.Featrue 
{
    public sealed class Entity
    {
        public int Eid { get; private set; }
        private List<Component> m_Components;

        internal Entity(int eid) 
        {
            Eid = eid;
            m_Components = new List<Component>();
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = GetComponent<T>();
            if (component == null) 
            {
                component = new();
                component.Init(this);
                m_Components.Add(component);
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

        internal void Destroy() 
        {
            foreach (var component in m_Components)
            {
                component.Destroy();             
            }
            m_Components.Clear();
            m_Components = null;
        }
    }
}