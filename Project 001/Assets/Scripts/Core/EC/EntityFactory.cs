using System.Collections.Generic;

namespace EC
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityFactory
    {
        private World world;

        private int index;

        private Dictionary<int, Entity> entities;

        private Queue<Entity> pool;

        internal EntityFactory(World world) 
        {
            this.world = world;
            index = 10000;
            entities = new Dictionary<int, Entity>();
            pool = new Queue<Entity>();
        }

        internal Entity CreateEntity(Entity parent = null) 
        {
            int parentGuid = parent != null ? parent.Guid : -1;
            int guid = GenerateGuid();
            Entity entity;

            if (pool.Count > 0) 
            {
                entity = pool.Dequeue();
                entity.Reuse(guid, parentGuid, world);
            }
            else
            {
                entity = new Entity(guid, parentGuid, world);
            }

            entities[guid] = entity;
            return entity;
        }

        internal void DestroyEntity(int guid)
        {          
            DestroyEntity(GetEntity(guid));
        }

        internal void DestroyEntity(Entity entity) 
        {
            if (entity == null || entity.IsValid == false) 
            {
                return;
            }

            int guid = entity.Guid;
            entity.Destroy();
            entities.Remove(guid);
            pool.Enqueue(entity);
        }

        internal Entity GetEntity(int guid) 
        {
            if (!entities.ContainsKey(guid)) 
            {
                return null;
            }
            else
            {
                return entities[guid];                
            }
        }

        private int GenerateGuid()
        {
            index++;
            return index;
        }
    }
}
