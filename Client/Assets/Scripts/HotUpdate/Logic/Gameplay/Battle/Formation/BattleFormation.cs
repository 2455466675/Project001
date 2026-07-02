using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace GameFramework.Logic
{
    public class BattleFormationSite : DataModel
    {
        private int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private BattleCamp camp;
        public BattleCamp Camp
        {
            get { return camp; }
            set { camp = value; }
        }

        private int state;
        public int State
        {
            get { return state; }
            set { SetValue(ref state, value); }
        }

        private bool valid;
        public bool Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
    }

    public class BattleFormation
    {
        private const string FormationCfgPath = "Assets/Bundles/Common/BattleFormationCfg";

        private const int PlayerCount = 4;
        private const int EnemyCount = 6;
        private List<BattleFormationSite> sites;

        public DataModelList<BattleFormationSite> GetFormationSites(BattleCamp camp)
        {
            DataModelList<BattleFormationSite> result = new DataModelList<BattleFormationSite>();
            for (int i = 0; i < sites.Count; i++)
            {
                var site = sites[i];
                if (site.Camp == camp)
                {
                    result.Add(site);
                }
            }
            result.Sort((a, b) => a.Index.CompareTo(b.Index));
            return result;
        }

        public BattleFormationSite GetFormationSite(BattleCamp camp, int index)
        {
            for (int i = 0; i < sites.Count; i++)
            {
                var site = sites[i];
                if (site.Camp == camp && site.Index == index)
                {
                    return site;
                }
            }
            return null;
        }

        public async UniTask LoadFormation(int formationID)
        {
            sites = new List<BattleFormationSite>(PlayerCount + EnemyCount);

            BattleFormationCfg cfg = await Game.Assets.LoadAssetAsync<BattleFormationCfg>(FormationCfgPath);
            BattleFormationCfg.FormationData formation = cfg != null ? cfg.GetFormation(formationID) : null;

            BuildSites(BattleCamp.Player, formation?.playerSites, PlayerCount);
            BuildSites(BattleCamp.Enemy, formation?.enemySites, EnemyCount);
            
            Game.Assets.ReleaseAsset(cfg);
        }

        public void UnloadFormation()
        {
            sites.Clear();
            sites = null;
        }

        private void BuildSites(BattleCamp camp, List<BattleFormationCfg.SiteData> configSites, int count)
        {
            configSites ??= new List<BattleFormationCfg.SiteData>();

            for (int i = 0; i < count; i++)
            {
                int index = i;
                BattleFormationCfg.SiteData data = configSites.Find(d => d.index == index);
                BattleFormationSite site = new BattleFormationSite();
                site.Index = i;
                site.Camp = camp;
                site.State = 0;
                site.Valid = data != null && data.valid;
                site.Position = data != null ? data.position : Vector3.zero;
                sites.Add(site);
            }
        }
    }
}
