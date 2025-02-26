namespace EC
{
    /// <summary>
    /// 
    /// </summary>
    public class Component
    {
        public int Guid { get; private set; }

        public Entity MyEntity { get; private set; }

        public World MyWorld => MyEntity?.MyWorld;

        public Component() { }

        internal void Initialize(Entity entity, int guid) 
        {
            MyEntity = entity;
            Guid = guid;
        }

        internal void Destroy() 
        {
            OnDestroy();
            Guid = -1;
            MyEntity = null;
        }

        public T GetComponent<T>() where T : Component 
        {
            if (MyEntity == null) 
            {
                return default;
            }
            else 
            {
                return MyEntity.GetComponent<T>();            
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
