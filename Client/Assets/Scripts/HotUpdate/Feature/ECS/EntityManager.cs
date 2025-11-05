using System.Collections.Generic;

namespace GameFramework.Featrue 
{
    [GameModule]
    public class EntityManager : IGameModule_SyncInit, IUpdate, IFixedUpdate
    {
        private int m_UidGenerator;
        private Dictionary<int, Entity> m_Entites;

        private HashSet<IUpdate> m_Updates;
        private HashSet<IUpdate> m_AddUpdates;
        private HashSet<IUpdate> m_DelUpdates;
        private HashSet<IFixedUpdate> m_FixedUpdates;
        private HashSet<IFixedUpdate> m_AddFixedUpdates;
        private HashSet<IFixedUpdate> m_DelFixedUpdates;

        public void Init()
        {
            m_UidGenerator = 10000;
            m_Entites = new Dictionary<int, Entity>();
            m_Updates = new HashSet<IUpdate>();
            m_AddUpdates = new HashSet<IUpdate>();
            m_DelUpdates = new HashSet<IUpdate>();
            m_FixedUpdates = new HashSet<IFixedUpdate>();
            m_AddFixedUpdates = new HashSet<IFixedUpdate>();
            m_DelFixedUpdates = new HashSet<IFixedUpdate>();
        }

        public void Update()
        {
            if (m_AddUpdates.Count > 0)
            {
                foreach (var item in m_AddUpdates)
                {
                    m_Updates.Add(item);                    
                }
                m_AddUpdates.Clear();
            }

            foreach (var item in m_Updates)
            {
                item.Update();
            }

            if (m_DelUpdates.Count > 0)
            {
                foreach (var item in m_DelUpdates)
                {
                    m_Updates.Remove(item);
                }
                m_DelUpdates.Clear();
            }
        }

        public void FixedUpdate()
        {
            if (m_AddFixedUpdates.Count > 0)
            {
                foreach (var item in m_AddFixedUpdates)
                {
                    m_FixedUpdates.Add(item);
                }
                m_AddFixedUpdates.Clear();
            }

            foreach (var item in m_FixedUpdates)
            {
                item.FixedUpdate();
            }

            if (m_DelFixedUpdates.Count > 0)
            {
                foreach (var item in m_DelFixedUpdates)
                {
                    m_FixedUpdates.Remove(item);
                }
                m_DelFixedUpdates.Clear();
            }
        }

        public Entity CreateEntity() 
        {
            int eid = m_UidGenerator++;
            Entity entity = new Entity(eid, RegisterComponent, UnregisterComponent);
            m_Entites.Add(eid, entity);
            return entity;
        }

        public Entity CreateEntity<T>() where T : Component, new()
        {
            int eid = m_UidGenerator++;
            Entity entity = new Entity(eid, RegisterComponent, UnregisterComponent);

            T component = entity.Internal_AddComponent<T>();
            component.Init(entity);
            RegisterComponent(component);

            m_Entites.Add(eid, entity);
            return entity;
        }

        public Entity CreateEntity<T0, T1>() where T0 : Component, new() where T1 : Component, new()
        {
            int eid = m_UidGenerator++;
            Entity entity = new Entity(eid, RegisterComponent, UnregisterComponent);

            Component c0 = entity.Internal_AddComponent<T0>();
            Component c1 = entity.Internal_AddComponent<T1>();
            c0.Init(entity);
            c1.Init(entity);
            RegisterComponent(c0);
            RegisterComponent(c1);

            m_Entites.Add(eid, entity);
            return entity;
        }

        public Entity CreateEntity<T0, T1, T2>() where T0 : Component, new() where T1 : Component, new() where T2 : Component, new()
        {
            int eid = m_UidGenerator++;
            Entity entity = new Entity(eid, RegisterComponent, UnregisterComponent);

            Component c0 = entity.Internal_AddComponent<T0>();
            Component c1 = entity.Internal_AddComponent<T1>();
            Component c2 = entity.Internal_AddComponent<T2>();
            c0.Init(entity);
            c1.Init(entity);
            c2.Init(entity);
            RegisterComponent(c0);
            RegisterComponent(c1);
            RegisterComponent(c2);

            m_Entites.Add(eid, entity);
            return entity;
        }

        public Entity CreateEntity<T0, T1, T2, T3>() where T0 : Component, new() where T1 : Component, new() where T2 : Component, new() where T3 : Component, new()
        {
            int eid = m_UidGenerator++;
            Entity entity = new Entity(eid, RegisterComponent, UnregisterComponent);

            Component c0 = entity.Internal_AddComponent<T0>();
            Component c1 = entity.Internal_AddComponent<T1>();
            Component c2 = entity.Internal_AddComponent<T2>();
            Component c3 = entity.Internal_AddComponent<T3>();
            c0.Init(entity);
            c1.Init(entity);
            c2.Init(entity);
            c3.Init(entity);
            RegisterComponent(c0);
            RegisterComponent(c1);
            RegisterComponent(c2);
            RegisterComponent(c3);

            m_Entites.Add(eid, entity);
            return entity;
        }

        public Entity CreateEntity<T0, T1, T2, T3, T4>() where T0 : Component, new() where T1 : Component, new() where T2 : Component, new() where T3 : Component, new() where T4 : Component, new()
        {
            int eid = m_UidGenerator++;
            Entity entity = new Entity(eid, RegisterComponent, UnregisterComponent);

            Component c0 = entity.Internal_AddComponent<T0>();
            Component c1 = entity.Internal_AddComponent<T1>();
            Component c2 = entity.Internal_AddComponent<T2>();
            Component c3 = entity.Internal_AddComponent<T3>();
            Component c4 = entity.Internal_AddComponent<T4>();
            c0.Init(entity);
            c1.Init(entity);
            c2.Init(entity);
            c3.Init(entity);
            c4.Init(entity);
            RegisterComponent(c0);
            RegisterComponent(c1);
            RegisterComponent(c2);
            RegisterComponent(c3);
            RegisterComponent(c4);

            m_Entites.Add(eid, entity);
            return entity;
        }

        public Entity GetEntity(int eid)
        {
            if (m_Entites.ContainsKey(eid)) 
            {
                return m_Entites[eid];
            }
            else
            {
                return null;
            }           
        }

        public void DestroyEntity(int eid)
        {
            if (!m_Entites.ContainsKey(eid))
            {
                return;
            }

            Entity entity = m_Entites[eid];
            entity.Destroy();
            m_Entites.Remove(eid);
        }

        private void RegisterComponent(Component component)
        {
            if (component is IUpdate update)
            {
                m_AddUpdates.Add(update);
            }

            if (component is IFixedUpdate fixedUpdate)
            {
                m_AddFixedUpdates.Add(fixedUpdate);
            }
        }

        private void UnregisterComponent(Component component)
        {
            if (component is IUpdate update)
            {
                m_DelUpdates.Add(update);
            }

            if (component is IFixedUpdate fixedUpdate)
            {
                m_DelFixedUpdates.Add(fixedUpdate);
            }
        }
    }
}
