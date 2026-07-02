using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Logic
{
    // 阵形配置资产：一份资产聚合所有阵形，运行时按 formationID 检索，避免每个阵形一个资产带来的碎片化管理
    public class BattleFormationCfg : ScriptableObject
    {
        [Serializable]
        public class SiteData
        {
            // 同阵营内的槽位序号，仅作标识用途
            public int index;
            // 该点位是否参与战斗（无效点位不放置单位）
            public bool valid = true;
            // 世界坐标，运行时直接赋给单位/表现节点的 position
            public Vector3 position;
        }

        [Serializable]
        public class FormationData
        {
            public int formationID;
            public List<SiteData> playerSites = new List<SiteData>();
            public List<SiteData> enemySites = new List<SiteData>();
        }

        [SerializeField]
        private List<FormationData> formations = new List<FormationData>();

        public List<FormationData> Formations
        {
            get
            {
                if (formations == null)
                {
                    formations = new List<FormationData>();
                }

                return formations;
            }
        }

        public FormationData GetFormation(int formationID)
        {
            if (formations == null)
            {
                return null;
            }

            return formations.Find(f => f.formationID == formationID);
        }
    }
}
