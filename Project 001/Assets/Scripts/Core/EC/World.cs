namespace EC
{
    /// <summary>
    /// 
    /// </summary>
    public class World
    {
        private EntityFactory entityFactory;
        private ComponentManager componentManager;
        public Entity Root { get; private set; }
  
        public World() 
        {
            entityFactory = new EntityFactory(this);
            componentManager = new ComponentManager();

            Root = entityFactory.CreateEntity();
        }

        internal Entity CreateEntity(Entity parent) 
        {
            return entityFactory.CreateEntity(parent);
        }

        public Entity GetEntity(int guid)
        {
            return entityFactory.GetEntity(guid);
        }

        public void DestroyEntity(int guid)
        {
            entityFactory.DestroyEntity(guid);
        }

        public void DestroyEntity(Entity entity) 
        {
            entityFactory.DestroyEntity(entity);
        }

        internal T CreateComponent<T>(Entity entity) where T : Component, new() 
        {
            return componentManager.CreateComponent<T>(entity);
        }

        public T AddComponent<T>() where T : Component, new() 
        {
            return Root.AddComponent<T>();
        }

        public T GetComponent<T>() where T : Component
        {
            return Root.GetComponent<T>();
        }

        public void DestroyComponent(Component component)
        {
            componentManager.DestroyComponent(component);
        }

        public void Update(float dt) 
        {
            componentManager.Update(dt);
        }
    }
}
