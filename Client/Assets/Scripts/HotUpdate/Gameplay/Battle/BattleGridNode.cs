using GameFramework.Core;
using UnityEngine;

namespace GameFramework.Gameplay
{
    public class BattleGridNode : MonoBehaviour
    {
        private GameObject m_GridObj;

        public void LoadGrid(int r, int c)
        {
            if (m_GridObj != null)
            {
                return;
            }
            m_GridObj = Game.GetModule<AssetsManager>().LoadAndInstantiate("Assets/Bundles/Battle/BattleGrid", transform);
        }

        public void DestroyGrid()
        {
            if (m_GridObj == null)
            {
                return;
            }

            m_GridObj.transform.SetParent(null);
            GoHelper.Destroy(m_GridObj);
            m_GridObj = null;
        }
    }
}
