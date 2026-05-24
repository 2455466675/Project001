using System;
using System.Collections.Generic;

namespace ECS
{
    public sealed class Entity : IEntity
    {
        public int Eid { get; private set; }

        int IEntity.Eid => throw new NotImplementedException();

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

        public T AddComponent<T>() where T : ComponentBase, new()
        {
            IAddComponent inst = this;
            return inst.AddComponent<T>();
        }
    }
}