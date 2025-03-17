using System;
using System.Collections.Generic;

namespace ECS
{
    /// <summary>
    /// 
    /// </summary>
    internal class EntityFactory
    {
        private static EntityFactory instance;
        internal static EntityFactory Instance
        {
            get 
            {                
                return instance;
            }
        }

        internal static void Initialize() 
        {
            if (instance != null)
            {
                return;
            }
            instance = new EntityFactory();
        }

        private int entityIndex;

        private Dictionary<int, Entity> entities;
        private HashSet<IUpdate> updateItems;
        private List<IUpdate> addUpdateCache;
        private List<IUpdate> removeUpdateCache;
        private Dictionary<Type, Queue<Entity>> pool;

        private EntityFactory() 
        {
            entityIndex = 10000;

            entities = new Dictionary<int, Entity>();
            updateItems = new HashSet<IUpdate>();
            addUpdateCache = new List<IUpdate>();
            removeUpdateCache = new List<IUpdate>();
            pool = new Dictionary<Type, Queue<Entity>>();
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

        internal T CreateEntity<T>(int parentGuid, bool isComponent) where T : Entity, new()
        {     
            Type t = typeof(T);

            T r = default;
            if (pool.TryGetValue(t, out Queue<Entity> queue) && queue.Count > 0)
            {
                r = queue.Dequeue() as T;                
            }

            r ??= new T();

            int guid = GenerateEntityGuid();
            r.Initialize(guid, parentGuid, isComponent);
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

        internal void DestroyEntity(int guid) 
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

        internal Entity FindEntity(int guid) 
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

        private void Recycle(Entity e)
        {
            Type type = e.GetType();
            Queue<Entity> queue;
            if (pool.ContainsKey(type))
            {
                queue = pool[type];
            }
            else
            {
                queue = new Queue<Entity>();
                pool[type] = queue;
            }

            queue.Enqueue(e);
        }

        private int GenerateEntityGuid() 
        {
            return ++entityIndex;
        }
    }
}
