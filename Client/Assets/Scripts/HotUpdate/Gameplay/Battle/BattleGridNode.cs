using GameFramework.Core;
using UnityEngine;

namespace GameFramework.Gameplay
{
    public class BattleGridNode : MonoBehaviour
    {
        private GameObject m_GridObj;

        public T LoadGrid<T>(int r, int c) where T : Component
        {
            if (m_GridObj != null)
            {
                return default;
            }
            m_GridObj = Game.GetModule<AssetsManager>().LoadAndInstantiate("Assets/Bundles/Battle/BattleGrid", transform);
            if (m_GridObj == null)
            {
                return default;
            }

            return m_GridObj.GetComponent<T>();
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
