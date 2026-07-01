using UnityEngine;

namespace GameFramework.EditorTools
{
    // 阵形编辑入口：挂在阵形根节点上，指定玩家/敌人两个父节点，其直接子物体即为对应阵营的各个点位
    // 采用“父节点 + 子物体”方式而非固定数组，是为了在场景中用常规层级操作即可增删点位
    // 归属 Editor 程序集，只服务于编辑期采集，不参与运行时逻辑
    public class BattleFormationAuthoring : MonoBehaviour
    {
        [SerializeField]
        private int formationID = 1;
        [SerializeField]
        private Transform playerRoot;
        [SerializeField]
        private Transform enemyRoot;

        public int FormationID
        {
            get { return formationID; }
            set { formationID = value; }
        }

        public Transform PlayerRoot
        {
            get { return playerRoot; }
        }

        public Transform EnemyRoot
        {
            get { return enemyRoot; }
        }
    }
}
