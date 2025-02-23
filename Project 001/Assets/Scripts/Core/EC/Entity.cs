using System;
using System.Collections.Generic;

namespace EC
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Entity : IComponentOperator
    {
        public int Guid { get; private set; }
        public bool IsValid { get; private set; }
        public World World { get; private set; }

        private int parentGuid;
        public Entity Parent => World.GetEntity(parentGuid);

        private List<int> children;
        public int ChildCount => children.Count;

        private Dictionary<Type, Component> components;

        internal Entity(int guid, int parent, World world)
        {       
            Reuse(guid, parent, world);
            components = new Dictionary<Type, Component>();
            children = new List<int>();
        }

        internal void Reuse(int guid, int parent, World world) 
        {
            Guid = guid;
            World = world;
            parentGuid = parent;
            IsValid = true;
        }

        internal void Destroy() 
        {
            foreach (var child in children)
            {
                World.DestroyEntity(child);
            }
            children.Clear();

            foreach (var component in components.Values)
            {
                World.DestroyComponent(component);
            }
            components.Clear();

            Guid = -1;
            parentGuid = -1;
            World = null;
            IsValid = false;
        }

        public Entity CreateChild() 
        {
            Entity entity = World.CreateEntity(this);
            children.Add(entity.Guid);
            return entity;
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

        public T AddComponent<T>() where T : Component, new()
        {
            Type t = typeof(T);
            if (components.ContainsKey(t)) 
            {
                return components[t] as T;
            }
            else
            {
                T component = World.CreateComponent<T>(this);           
                components[t] = component;
                return component;
            }
        }

        public void RemoveComponent<T>() 
        {
            Type t = typeof(T);
            if (components.TryGetValue(t, out Component v)) 
            { 
                World.DestroyComponent(v);
                components.Remove(t);
            }
        }

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
