using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Entity
    {
        public int Guid { get; private set; }
        public bool IsComponent { get; private set; }
        public int ChildCount => children.Count;
        public Entity Parent => EntityFactory.Instance.FindEntity(parent);

        private int parent;
        private HashSet<int> children;
        private Dictionary<Type, int> components;
        
        internal void Initialize(int guid, int parentGuid, bool isCompoent) 
        {
            Guid = guid;
            parent = parentGuid;
            IsComponent = isCompoent;
            children = new HashSet<int>();
            components = new Dictionary<Type, int>();
        }

        internal void Destroy()
        {
            OnDestroy();

            foreach (var guid in components.Values)
            {
                EntityFactory.Instance.DestroyEntity(guid);
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

        public T AddComponent<T>() where T : Entity, new()
        {
            Type t = typeof(T);
            if (components.ContainsKey(t)) 
            {
                Entity e = EntityFactory.Instance.FindEntity(components[t]);
                return e as T;
            }
            else
            {
                T c = EntityFactory.Instance.CreateEntity<T>(this.Guid, true);
                components.Add(t, c.Guid);
                return c;
            }
        }

        public T GetComponent<T>() where T : Entity
        {
            Type t = typeof(T);
            if (components.ContainsKey(t))
            {
                Entity e = EntityFactory.Instance.FindEntity(components[t]);
                return e as T;
            }
            else
            {
                return default;
            }
        }
        
        public void RemoveComponent<T>() where T : Entity
        {
            Type t = typeof(T);
            if (components.ContainsKey(t))
            {
                EntityFactory.Instance.DestroyEntity(components[t]);
                components.Remove(t);
            }
        }

        public T CreateChild<T>() where T : Entity, new()
        {
            T child = EntityFactory.Instance.CreateEntity<T>(this.Guid, false);
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
