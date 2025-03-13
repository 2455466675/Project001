namespace ECS
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Component : IEntity
    {
        public int Guid { get; private set; }
        public Entity Entity => EntityFactory.Instance.FindEntity(entity);

        private int entity;

        internal Component() { }

        internal void Initialize(int guid, int parentGuid)
        {
            Guid = guid;
            entity = parentGuid;
        }

        internal void Destroy()
        {
            OnDestroy();            
        }

        public T AddComponent<T>() where T : Component, new()
        {
            Entity e = this.Entity;
            if (e == null) 
            {
                return default;
            }
            else
            {
                return e.AddComponent<T>();                
            }
        }

        public T GetComponent<T>() where T : Component
        {
            Entity e = this.Entity;
            if (e == null)
            {
                return default;
            }
            else
            {
                return e.GetComponent<T>();
            }
        }

        public void RemoveComponent<T>() where T : Component
        {
            Entity e = this.Entity;
            if (e == null)
            {
                return;
            }
            e.RemoveComponent<T>();
        }

        public virtual void OnDestroy() { }

        public override int GetHashCode()
        {
            return Guid;
        }

        public override bool Equals(object obj)
        {
            return obj is Component cp && cp.Guid == Guid;
        }
    }
}
