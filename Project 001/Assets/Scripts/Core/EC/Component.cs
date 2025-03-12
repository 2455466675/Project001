namespace EC
{
    /// <summary>
    /// 
    /// </summary>
    public class Component
    {
        public int Guid { get; private set; }

        public Entity Entity { get; private set; }

        public World World => Entity?.World;

        public Component() { }

        internal void Initialize(Entity entity, int guid) 
        {
            Entity = entity;
            Guid = guid;
        }

        internal void Destroy() 
        {
            OnDestroy();
            Guid = -1;
            Entity = null;
        }

        public T GetComponent<T>() where T : Component 
        {
            if (Entity == null) 
            {
                return default;
            }
            else 
            {
                return Entity.GetComponent<T>();            
            }
        }

        public override int GetHashCode()
        {
            return Guid;
        }

        public override bool Equals(object obj)
        {
            return obj is Component cp && cp.Guid == Guid;
        }

        protected virtual void OnDestroy() { }
    }
}
