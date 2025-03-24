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
        public static T FindEntity<T>(int guid) where T : Entity
        {
            if (EntityFactory.Instance == null) 
            {
                return default;
            }

            Entity e = EntityFactory.Instance.FindEntity(guid);
            if (e == null)
            {
                return default;
            }
            else
            {
                return e as T;
            }
        }

        protected readonly Entity root;

        public World() 
        {
            EntityFactory.Initialize();
            root = EntityFactory.Instance.CreateEntity<Root>(0, false);
        }

        public void Update(float dt) 
        {
            EntityFactory.Instance.Update(dt);
        }
    }
}
