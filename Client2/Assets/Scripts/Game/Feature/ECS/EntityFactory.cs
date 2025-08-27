using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameFramework.Featrue 
{
    public class EntityFactory : IGameModule
    {
        private int m_UidGenerator;
        private Dictionary<int, Entity> m_Entites;

        public GameModulePriority Priority => GameModulePriority.EntityManager;

        public async UniTask Init()
        {
            m_UidGenerator = 10000;
            m_Entites = new Dictionary<int, Entity>();

            await UniTask.Yield();
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
