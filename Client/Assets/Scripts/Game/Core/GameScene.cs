using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace Game
{
    public struct SceneLoadingProgress
    {
        public float progress;
    }

    public class GameScene
    {
        private const int MinLoadFrame = 20;

        private SceneMap sceneMap;
        private List<SceneEntity> scenes;

        private SceneEntity mainScene;
        private SceneEntity battleScene;

        private int loadingSceneId;
        private int loadFrameCount;

        public void Init()
        {
            loadingSceneId = -1;
            scenes = new List<SceneEntity>();
            sceneMap = Game.Resource.LoadFormRes<SceneMap>(Game.Config.Formula.SceneMap);
        }
        public void LoadBattleScene() 
        {
            Game.System.ActorManager.SetActive(false);
            mainScene.SetActive(false);
            battleScene.SetActive(true);
            SceneManager.SetActiveScene(battleScene.Scene);
        }

        public void UnloadBattleScene() 
        {
            SceneManager.SetActiveScene(mainScene.Scene);
            battleScene.SetActive(false);
            mainScene.SetActive(true);
            Game.System.ActorManager.SetActive(true);
        }

        public async UniTask PreloadBattleScene()
        {
            SceneEntity entity = FindOrCreateSceneEntity(10002);            
            await entity.LoadSceneAsync(null);
            entity.SetActive(false);
            battleScene = entity;
        }

        public void LoadScene(int sceneId)
        {
            if (loadingSceneId > 0)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneId);
                return;
            }
            if (mainScene != null && mainScene.SceneId == sceneId)
            {
                return;
            }
            SceneEntity entity = FindOrCreateSceneEntity(sceneId);
            entity.LoadScene();            
            mainScene?.UnloadScene();
            mainScene = entity;
        }

        public async UniTaskVoid LoadSceneAsync(int sceneId, Action loadStartAction, Action loadEndAction, List<UniTask> tasks, int minLoadFrame = MinLoadFrame) 
        {
            if (loadingSceneId > 0)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneId);
                return;
            }

            if (mainScene != null && mainScene.SceneId == sceneId) 
            {
                return;
            }

            loadStartAction?.Invoke();

            loadingSceneId = sceneId;
            loadFrameCount = 0;

            SceneEntity entity = FindOrCreateSceneEntity(sceneId);
            await entity.LoadSceneAsync((p) =>
            {
                TickLoadProgress(minLoadFrame, (int)(minLoadFrame * 0.5f), 1);
            });

            mainScene?.UnloadScene();
            mainScene = entity;

            if (tasks != null && tasks.Count > 0) 
            {
                int step = GameMathf.Max(1, (minLoadFrame - loadFrameCount) / tasks.Count);

                for (int i = 0; i < tasks.Count; i++)
                {
                    UniTask task = tasks[i];
                    await task;
                    TickLoadProgress(minLoadFrame, (int)(minLoadFrame * 0.9f), step);
                }                
            }

            while (loadFrameCount < minLoadFrame)
            {
                await UniTask.Yield();
                TickLoadProgress(minLoadFrame, minLoadFrame, 1);
            }

            loadingSceneId = -1;
            loadEndAction?.Invoke();
        }

        private void TickLoadProgress(int max, int limit, int step) 
        {
            loadFrameCount = GameMathf.Min(limit, loadFrameCount + step);
            float progress = loadFrameCount * 1.0f / max;
            Game.Event.Publish(new SceneLoadingProgress() { progress = progress });
        }
    
        private SceneEntity FindOrCreateSceneEntity(int sceneId)
        {
            SceneEntity entity = scenes.Find(e => e.SceneId == sceneId);
            if (entity == null)
            {
                entity = new SceneEntity();
                entity.Init(sceneMap.GetSceneData(sceneId));

                scenes.Add(entity);
            }
            return entity;
        }
    }
}