using UnityEngine;

namespace GameFramework.EditorTools
{
    // 阵形点位的编辑期标记：位置直接取自 Transform，有效性单独存储，仅用于编辑阶段摆放与勾选
    // 归属 Editor 程序集，不进入运行时热更逻辑；运行时只消费采集后的 BattleFormationCfg
    public class BattleFormationPoint : MonoBehaviour
    {
        [SerializeField]
        private bool valid = true;

        public bool Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        // 用颜色区分有效/无效点位，便于直观校对阵形
        private void OnDrawGizmos()
        {
            Gizmos.color = valid ? new Color(0.2f, 0.9f, 0.3f, 1f) : new Color(0.9f, 0.2f, 0.2f, 1f);
            Gizmos.DrawWireSphere(transform.position, 0.2f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 0.5f);
        }
    }
}
