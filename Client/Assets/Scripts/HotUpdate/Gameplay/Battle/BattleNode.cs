using GameFramework.Core;
using UnityEngine;

namespace GameFramework.Gameplay
{
    public class BattleNode : GameNode
    {
        [SerializeField]
        private BattleGridNode m_GridNode;

        public void LoadGrid(int r, int c)
        {
            if (m_GridNode != null)
            {
                m_GridNode.LoadGrid(r, c);
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
