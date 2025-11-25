using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace GameFramework.Core 
{
    [GameModule]
    public class SceneManager : IGameModule_AsyncInit
    {
        private Dictionary<int, SceneEntity> m_Entities;

        private SceneEntity m_MainScene;
        private SceneEntity m_BattleScene;

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
         * 
         * Init => Login <=> Play
         *              
         * LoadView
         * 
         * None
         * First
         * Normal
         * Battle
         * 
        */

        public async UniTask LoadScene(int id) 
        {
            if (m_MainScene != null && m_MainScene.SceneId == id) 
            {                
                return;
            }

            if (!m_Entities.ContainsKey(id)) 
            {
                return;
            }

            SceneEntity entity = m_Entities[id];
            await entity.LoadAsync();

            if (m_MainScene != null) 
            {
                await m_MainScene.UnloadAsync();            
            }

            m_MainScene = entity;
            m_MainScene.ActivateScene();

            //await Game.GetModule<AssetsManager>().UnloadUnusedAssetsAsync();
        }

        public async UniTask LoadBattleScene()
        {
            if (m_BattleScene != null && m_BattleScene.IsValid)
            {
                return;
            }

            SceneEntity entity = m_Entities[3001];
            await entity.LoadAsync();

            entity.SetVisible(false);
            m_BattleScene = entity;
        }

        public void EnterBattleScene()
        {
            if (m_BattleScene == null || !m_BattleScene.IsValid)
            {
                return;
            }

            if (m_MainScene == null || !m_MainScene.IsValid)
            {
                return;
            }

            m_BattleScene.SetVisible(true);
            m_MainScene.SetVisible(false);
        }

        public void ExitBattleScene()
        {
            if (m_BattleScene == null || !m_BattleScene.IsValid)
            {
                return;
            }

            if (m_MainScene == null || !m_MainScene.IsValid)
            {
                return;
            }

            m_BattleScene.SetVisible(false);
            m_MainScene.SetVisible(true);
        }
    }
}

