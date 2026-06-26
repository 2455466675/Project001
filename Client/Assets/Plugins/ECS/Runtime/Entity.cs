using System;
using System.Collections.Generic;

namespace ECS
{
    public sealed class Entity : IEntity
    {
        public int Eid { get; private set; }

        int IEntity.Eid => this.Eid;

        private List<IComponent> components;
        private IWorld componentFactory;

        internal Entity()
        {
        }

        internal void AddComponent(IComponent component)
        {
            components.Add(component);
        }

        void IEntity.Init(int eid, IWorld componentFactory)
        {
            Eid = eid;
            components = new List<IComponent>();
            this.componentFactory = componentFactory;
        }

        void IEntity.Destroy()
        {
            foreach (var component in components)
            {
                componentFactory.DestroyComponent(component);
            }
            components.Clear();
            components = null;
            componentFactory = null;
        }

        T IAddComponent.AddComponent<T>()
        {
            T component = componentFactory.CreateComponent<T>(this);
            AddComponent(component);
            component.Init(this);
            component.Start();
            return component;
        }

        T IGetComponent.GetComponent<T>()
        {
            for (int i = 0; i < components.Count; i++)
            {
                IComponent component = components[i];
                if (component is T)
                {
                    return component as T;
                }
            }

            return default;
        }

        public T GetComponent<T>() where T : ComponentBase
        {
            IGetComponent inst = this;
            return inst.GetComponent<T>();
        }

        public ComponentBase GetComponent(Type type)
        {
            for (int i = 0; i < components.Count; i++)
            {
                IComponent component = components[i];
                if (component.GetType() == type)
                {
                    return component as ComponentBase;
                }
            }
            return null;
        }

        public T AddComponent<T>() where T : ComponentBase, new()
        {
            IAddComponent inst = this;
            return inst.AddComponent<T>();
        }

        public ComponentBase AddComponent(Type type)
        {
            IComponent component = componentFactory.CreateComponent(type);
            if (component == null)
            {
                return null;
            }
            AddComponent(component);
            component.Init(this);
            component.Start();
            return component as ComponentBase;
        }
    }
}