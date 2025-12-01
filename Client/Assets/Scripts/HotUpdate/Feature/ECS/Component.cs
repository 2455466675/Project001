namespace GameFramework.Featrue 
{
    public abstract class Component : IGetComponent
    {
        public int Eid => m_Entity != null ? m_Entity.Eid : -1;
        private Entity m_Entity;

        internal void Init(Entity entity)
        {
            m_Entity = entity;
            OnInit();
        }

        internal void Destroy()
        {
            OnDestroy();
            m_Entity = null;
        }

        public T GetComponent<T>() where T : Component
        {
            if (m_Entity == null)
            {
                return default;
            }
            else
            {
                return m_Entity.GetComponent<T>();                
            }
        }

        protected virtual void OnInit() 
        {
        }

        protected virtual void OnDestroy()
        {
        }
    }
}
