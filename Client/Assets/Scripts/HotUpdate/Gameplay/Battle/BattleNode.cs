using GameFramework.Core;
using UnityEngine;

namespace GameFramework.Gameplay
{
    public class BattleNode : GameNode
    {
        [SerializeField]
        private BattleGridNode m_GridNode;

        public T LoadGrid<T>(int r, int c) where T : Component
        {
            if (m_GridNode != null)
            {
                return m_GridNode.LoadGrid<T>(r, c);
            }
            else
            {
                return default;
            }
        }

        public void DestroyGrid()
        {
            if (m_GridNode != null)
            {
                m_GridNode.DestroyGrid();
            }
        }
    }
}
