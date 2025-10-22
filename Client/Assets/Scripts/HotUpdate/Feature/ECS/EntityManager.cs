using System.Collections.Generic;

namespace GameFramework.Featrue 
{
    [GameModule(GameModulePriority.EntityManager)]
    public class EntityManager : IGameModule_SyncInit
    {
        private int m_UidGenerator;
        private Dictionary<int, Entity> m_Entites;

        public void Init()
        {
            m_UidGenerator = 10000;
            m_Entites = new Dictionary<int, Entity>();
        }

        public Entity CreateEntity() 
        {
            int eid = m_UidGenerator++;
            Entity entity = new Entity(eid);
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
    }
}
