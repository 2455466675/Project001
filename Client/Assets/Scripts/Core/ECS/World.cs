namespace ECS
{
    internal class Root : Entity
    {    
    }

    /// <summary>
    /// 
    /// </summary>
    public class World
    {
        protected readonly Entity root;

        public World() 
        {
            EntityFactory.Initialize();
            root = EntityFactory.Instance.CreateEntity<Root>(0);
        }

        public void Update(float dt) 
        {
            EntityFactory.Instance.Update(dt);
        }
    }
}
