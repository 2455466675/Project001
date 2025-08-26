namespace GameFramework.Featrue 
{
    public class Component
    {
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
            return m_Entity.GetComponent<T>();
        }

        protected virtual void OnInit() 
        {
        }

        protected virtual void OnDestroy()
        {
        }
    }
}
