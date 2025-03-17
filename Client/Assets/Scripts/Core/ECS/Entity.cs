using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Entity : IEntity
    {
        public int Guid { get; private set; }
        public int ChildCount => children.Count;
        public Entity Parent => EntityFactory.Instance.FindEntity(parent);

        private int parent;
        private HashSet<int> children;
        private Dictionary<Type, Component> components;
        
        internal void Initialize(int guid, int parentGuid) 
        {
            Guid = guid;
            parent = parentGuid;
            children = new HashSet<int>();
            components = new Dictionary<Type, Component>();
        }

        internal void Destroy()
        {
            OnDestroy();

            foreach (var component in components.Values)
            {
                EntityFactory.Instance.DestroyComponent(component);
            }
            components.Clear();
            components = null;

            foreach (var guid in children)
            {
                EntityFactory.Instance.DestroyEntity(guid);
            }

            children.Clear();
            children = null;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            Type t = typeof(T);
            if (components.ContainsKey(t)) 
            {
                return components[t] as T;
            }
            else
            {
                T c = EntityFactory.Instance.CreateComponent<T>(this.Guid);
                components.Add(t, c);
                return c;
            }
        }

        public T GetComponent<T>() where T : Component
        {
            Type t = typeof(T);
            if (components.ContainsKey(t))
            {
                return components[t] as T;
            }
            else
            {
                return default;
            }
        }
        
        public void RemoveComponent<T>() where T : Component
        {
            Type t = typeof(T);
            if (components.ContainsKey(t))
            {
                Component component = components[t];
                EntityFactory.Instance.DestroyComponent(component);
                components.Remove(t);
            }
        }

        public T CreateChild<T>() where T : Entity, new()
        {
            T child = EntityFactory.Instance.CreateEntity<T>(this.Guid);
            children.Add(child.Guid);
            return child;
        }

        public T FindChild<T>() where T : Entity
        {
            foreach (var guid in children)
            {
                Entity child = EntityFactory.Instance.FindEntity(guid);
                if (child == null)
                {
                    continue;
                }

                if (child is T)
                {
                    return child as T;
                }
            }

            return default;
        }

        public T FindChild<T>(int guid) where T : Entity
        {
            if (!children.Contains(guid))
            {
                return default;
            }

            Entity child = EntityFactory.Instance.FindEntity(guid);
            if (child == null)
            {
                return default;
            }

            return child as T;
        }

        public void RemoveChild(int guid) 
        {
            if (!children.Contains(guid))
            {
                return;
            }
            children.Remove(guid);
            EntityFactory.Instance.DestroyEntity(guid);
        }

        protected virtual void OnDestroy() { }

        public override int GetHashCode()
        {
            return Guid;
        }

        public override bool Equals(object obj)
        {
            return obj is Entity e && e.Guid == Guid;
        }
    }
}
