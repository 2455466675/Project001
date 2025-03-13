using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityFactory
    {
        private static EntityFactory instance;
        public static EntityFactory Instance
        {
            get 
            {
                if (instance == null) 
                {
                    instance = new EntityFactory();
                }
                return instance;
            }
        }

        private int entityIndex;
        private int componentIndex;

        private Dictionary<int, Entity> entities;
        private HashSet<IUpdate> updateItems;
        private List<IUpdate> addUpdateCache;
        private List<IUpdate> removeUpdateCache;
        private Dictionary<Type, Queue<IEntity>> pool;

        private EntityFactory() 
        {
            entityIndex = 10000;
            componentIndex = 20000;

            entities = new Dictionary<int, Entity>();
            updateItems = new HashSet<IUpdate>();
            addUpdateCache = new List<IUpdate>();
            removeUpdateCache = new List<IUpdate>();
            pool = new Dictionary<Type, Queue<IEntity>>();
        }

        public void Update(float dt) 
        {
            if (addUpdateCache.Count > 0) 
            {
                for (int i = 0; i < addUpdateCache.Count; i++)
                {
                    updateItems.Add(addUpdateCache[i]);
                }
                addUpdateCache.Clear();
            }
            
            if (removeUpdateCache.Count > 0) 
            {
                for (int i = 0; i < removeUpdateCache.Count; i++)
                {
                    updateItems.Remove(removeUpdateCache[i]);
                }
                removeUpdateCache.Clear();
            } 
            
            foreach (var item in updateItems)
            {
                item.Update(dt);
            }
        }

        public T CreateEntity<T>(int parentGuid) where T : Entity, new()
        {     
            Type t = typeof(T);

            T r = default;
            if (pool.TryGetValue(t, out Queue<IEntity> queue) && queue.Count > 0)
            {
                r = queue.Dequeue() as T;                
            }

            r ??= new T();

            int guid = GenerateEntityGuid();
            r.Initialize(guid, parentGuid);
            entities[guid] = r;

            if (r is IAwake a) 
            {
                a.Awake();
            }

            if (r is IUpdate u) 
            {
                addUpdateCache.Add(u);
            }

            return r;
        }

        public void DestroyEntity(int guid) 
        {
            Entity entity = FindEntity(guid);
            if (entity == null) 
            {
                return;
            }

            if (entity is IUpdate u)
            {
                removeUpdateCache.Remove(u);
            }

            entity.Destroy();
            Recycle(entity);
        }

        public Entity FindEntity(int guid) 
        {
            if (entities.ContainsKey(guid)) 
            {
                return entities[guid];
            }
            else
            {
                return null;
            }
        }

        internal T CreateComponent<T>(int parentGuid) where T : Component, new() 
        {
            Type t = typeof(T);

            T r = default;
            if (pool.TryGetValue(t, out Queue<IEntity> queue) && queue.Count > 0)
            {
                r = queue.Dequeue() as T;
            }

            r ??= new T();

            int guid = GenerateComponentGuid();
            r.Initialize(guid, parentGuid);

            if (r is IAwake a)
            {
                a.Awake();
            }

            if (r is IUpdate u)
            {
                addUpdateCache.Add(u);
            }

            return r;
        }

        internal void DestroyComponent(Component component) 
        {
            if (component == null)
            {
                return;
            }

            if (component is IUpdate u)
            {
                removeUpdateCache.Remove(u);
            }

            component.Destroy();
            Recycle(component);
        }

        private void Recycle(IEntity e)
        {
            Type type = e.GetType();
            Queue<IEntity> queue;
            if (pool.ContainsKey(type))
            {
                queue = pool[type];
            }
            else
            {
                queue = new Queue<IEntity>();
                pool[type] = queue;
            }

            queue.Enqueue(e);
        }

        private int GenerateEntityGuid() 
        {
            return ++entityIndex;
        }

        private int GenerateComponentGuid()
        {
            return ++componentIndex;
        }
    }
}
