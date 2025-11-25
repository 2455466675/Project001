using System.Collections.Generic;

namespace GameFramework.Gameplay
{
    public class BattleFlowManager
    {
        private List<int> m_AllUnits;
        private List<int> m_ReadyUnits;
        private int m_Actioning;

        public BattleFlowManager()
        {
            m_AllUnits = new List<int>();
            m_ReadyUnits = new List<int>();
        }

        public void AddUnit(int eid)
        {
            m_AllUnits.Add(eid);
        }

        public void RemoveUnit(int eid)
        {
            m_AllUnits.Remove(eid);
            m_ReadyUnits.Remove(eid);
        }
    }
}
