using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core 
{
    [GameModule(GameModulePriority.SceneManager)]
    public class SceneManager : IAsyncInit
    {
        private Dictionary<int, SceneEntity> m_Entities;

        public async UniTask Init()
        {
            SceneMap map = await Game.GetModule<AssetsManager>().LoadAssetAsync<SceneMap>("Assets/Bundles/Common/SceneMap");
            List<SceneConfig> configs = map.GetSceneConfigList();
            m_Entities = new Dictionary<int, SceneEntity>(configs.Count);

            for (int i = 0; i < configs.Count; i++)
            {
                SceneConfig cfg = configs[i];
                SceneEntity entity = new SceneEntity(cfg);
                m_Entities[cfg.Id] = entity;
            }
        }

        /*
         * LoadView
         * 
         * None
         * First
         * Normal
         * Battle
         * 
        */

        public void LoadScene(int id) 
        {
            if (m_Entities.TryGetValue(id, out var entity))
            {
                entity.Load();
            }
        }
    }
}

