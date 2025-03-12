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

        private List<Component> components;

        internal Entity(int guid, int parent, World world)
        {       
            Reuse(guid, parent, world);
            components = new List<Component>();
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

            foreach (var component in components)
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
            foreach (var component in components)
            {
                if (component is T)
                {
                    return component as T;
                }
            }
            return default;
        }

        public List<T> GetComponents<T>() where T : Component
        {
            List<T> components = new List<T>();

            foreach (var component in components)
            {
                if (component is not null)
                {
                    components.Add(component);
                }
            }

            return components;
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = World.CreateComponent<T>(this);
            components.Add(component);
            return component;
        }

        public void RemoveComponent<T>() 
        {
            List<Component> temp = new List<Component>();

            for (int i = 0; i < components.Count; i++)
            {
                Component component = components[i];
                if (component is T) 
                {
                    temp.Add(component);
                }
            }

            for (int i = 0; i < temp.Count; i++)
            {
                Component component = temp[i];
                World.DestroyComponent(component);
                components.Remove(component);
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
