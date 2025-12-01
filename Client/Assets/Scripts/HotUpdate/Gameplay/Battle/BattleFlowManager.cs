using GameFramework.Featrue;
using System.Collections.Generic;

namespace GameFramework.Gameplay
{
    public class BattleFlowManager
    {
        public int Actioning { get; set; }

        private List<int> m_AllUnits;
        private List<int> m_ReadyUnits;

        private List<BattleStateComponent> m_Components;

        public BattleFlowManager()
        {
            m_AllUnits = new List<int>();
            m_ReadyUnits = new List<int>();
            m_Components = new List<BattleStateComponent>();
        }

        public void StartUp()
        {
            foreach (var item in m_Components)
            {
                item.StartUp();
            }
        }

        public void Tick()
        {
            if (Actioning <= 0)
            {
                Actioning = DequeueReady();
            }

            foreach (var item in m_Components)
            {
                item.Tick();
            }
        }

        public void AddUnit(Entity entity)
        {
            var buc = entity.GetComponent<BattleUnitComponent>();
            var bsc = entity.GetComponent<BattleStateComponent>();

            m_AllUnits.Add(buc.BattleId);
            m_Components.Add(bsc);
        }

        public void RemoveUnit(int battleId)
        {
            m_AllUnits.Remove(battleId);
            m_ReadyUnits.Remove(battleId);
        }

        public void EnqueueReady(int battleId)
        {
            m_ReadyUnits.Add(battleId);
        }

        public int DequeueReady()
        {
            if (m_ReadyUnits.Count == 0)
            {
                return 0;
            }
            else
            {
                int r = m_ReadyUnits[0];
                m_ReadyUnits.Remove(0);
                return r;                
            }
        }

        public bool RemoveReady(int battleId)
        {
            return m_ReadyUnits.Remove(battleId);
        }
    }
}
