namespace ECS
{
    public abstract class ComponentBase : IComponent
    {
        public int Eid => entity.Eid;

        private IEntity entity;

        public ComponentBase() { }

        void IComponent.Init(IEntity entity)
        {
            this.entity = entity;
            Awake();
        }

        void IComponent.Start()
        {
            Start();
        }

        void IComponent.Destroy()
        {
            OnDestroy();
            entity = null;
        }

        T IGetComponent.GetComponent<T>()
        {
            return entity.GetComponent<T>();
        }

        public T GetComponent<T>() where T : ComponentBase
        {
            IGetComponent inst = this;
            return inst.GetComponent<T>();
        }

        /// <summary>
        /// 组件被附加到实体时触发
        /// </summary>
        protected virtual void Awake() { }

        protected virtual void Start() { }

        /// <summary>
        /// 组件被销毁时触发
        /// </summary>
        protected virtual void OnDestroy() { }
    }
}