using Config;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace GameFramework.Core
{
    [GameSystem]
    public class GameSceneSystem : IGameSystem, IAsyncInit
    {
        private Dictionary<int, SceneEntity> entities;

        private SceneEntity basicScene;
        private SceneEntity mainScene;
        private SceneEntity overlayScene;

        async UniTask IAsyncInit.Init()
        {
            entities = new Dictionary<int, SceneEntity>();
            await UniTask.CompletedTask;
        }

        public async UniTask LoadScene(int id)
        {
            SceneEntity entity = GetOrCreateSceneEntity(id);
            if (entity == null)
            {
                return;
            }

            if (entity.SceneMode == SceneMode.Basic)
            {
                if (basicScene != null)
                {
                    return;
                }

                await entity.LoadAsync();
                entity.ActivateScene();
                basicScene = entity;
            }

            if (entity.SceneMode == SceneMode.Single)
            {
                if (mainScene != null && mainScene.SceneId == id)
                {
                    return;
                }
                if (overlayScene != null)
                {
                    MDebug.Warn("当前有附加场景激活时，禁止加载其他主场景!");
                    return;
                }

                await entity.LoadAsync();
                entity.ActivateScene();

                if (mainScene != null)
                {
                    await mainScene.UnloadAsync();                    
                }
                mainScene = entity;

                await Game.Assets.UnloadUnusedAssetsAsync();
            }

            if (entity.SceneMode == SceneMode.Overlay)
            {
                if (overlayScene != null && overlayScene.SceneId == id)
                {
                    return;
                }

                await entity.LoadAsync();
                entity.ActivateScene();

                if (overlayScene != null)
                {
                    await overlayScene.UnloadAsync();
                }
                mainScene?.SetVisible(false);
                overlayScene = entity;
            }            
        }

        /// <summary>
        /// 卸载一个附加场景（mainScene无法通过此方法卸载，只能通过加载另一个mainScene将其顶掉）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async UniTask UnloadScene(int id)
        {
            if (overlayScene == null || overlayScene.SceneId != id)
            {
                return;
            }
            mainScene?.ActivateScene();
            await overlayScene.UnloadAsync();
            await Game.Assets.UnloadUnusedAssetsAsync();
            mainScene?.SetVisible(true);
            overlayScene = null;
        }

        /// <summary>
        /// 卸载所有场景（保留基础场景。一般回到登录界面调用）
        /// </summary>
        /// <returns></returns>
        public async UniTask UnloadAllScene()
        {
            if (mainScene != null)
            {
                await mainScene.UnloadAsync();
                mainScene = null;
            }
            if (overlayScene != null)
            {
                await overlayScene.UnloadAsync();
                overlayScene = null;
            }

            basicScene?.ActivateScene();
        }

        private SceneEntity GetOrCreateSceneEntity(int id)
        {
            if (!entities.TryGetValue(id, out SceneEntity entity))
            {
                var cfg = Game.Config.Find<SceneCfg>(id);
                if (cfg == null)
                {
                    MDebug.Error($"场景配置不存在! id = {id}");
                    return null;
                }
                entity = new SceneEntity(cfg);
                entities.Add(id, entity);
            }
            return entity;
        }
    }
}
