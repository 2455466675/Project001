using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace GameFramework.Core
{
    [GameSystem]
    public class GameSceneSystem : IGameSystem, IAsyncInit
    {
        private Dictionary<int, SceneEntity> entities;

        private SceneEntity mainScene;
        private SceneEntity battleScene;

        async UniTask IAsyncInit.Init()
        {
            SceneConfigMap map = await Game.Resources.LoadAssetAsync<SceneConfigMap>("Assets/Bundles/Common/SceneMap");
            List<SceneConfig> configs = map.GetConfigList();
            entities = new Dictionary<int, SceneEntity>(configs.Count);

            for (int i = 0; i < configs.Count; i++)
            {
                SceneConfig cfg = configs[i];
                SceneEntity entity = new SceneEntity(cfg);
                entities[cfg.Id] = entity;
            }
        }

        public async UniTask LoadScene(int id)
        {
            if (mainScene != null && mainScene.SceneId == id)
            {
                return;
            }

            if (!entities.ContainsKey(id))
            {
                return;
            }

            SceneEntity entity = entities[id];
            await entity.LoadAsync();

            if (mainScene != null)
            {
                await mainScene.UnloadAsync();
            }

            mainScene = entity;
            mainScene.ActivateScene();

            //await Game.GetModule<AssetsManager>().UnloadUnusedAssetsAsync();
        }

        public async UniTask LoadBattleScene()
        {
            if (battleScene != null && battleScene.IsValid)
            {
                return;
            }

            SceneEntity entity = entities[1003];
            await entity.LoadAsync();

            entity.SetVisible(false);
            battleScene = entity;
        }

        public void SetBattleSceneVisable(bool visable)
        {
            if (battleScene == null || !battleScene.IsValid)
            {
                return;
            }

            if (mainScene == null || !mainScene.IsValid)
            {
                return;
            }

            battleScene.SetVisible(visable);
            mainScene.SetVisible(!visable);
        }
    }
}
