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
        private SceneMap sceneMap;
        private List<SceneEntity> scenes;
        private Stack<SceneEntity> activatedScenes;

        private Action<float> loadingAction;
        private int loadingSceneId;

        public void Init(GameInitConfig config)
        {
            loadingSceneId = -1;

            scenes = new List<SceneEntity>();
            activatedScenes = new Stack<SceneEntity>();

            sceneMap = Game.Resource.LoadFormRes<SceneMap>(config.SceneMap);
        }

        public void LoadScene(int sceneId)
        {
            if (loadingSceneId > 0)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneId);
                return;
            }

            SceneEntity entity = FindOrCreateSceneEntity(sceneId);

            if (!entity.Scene.isLoaded)
            {
                Game.Resource.LoadScene(entity.Path, entity.LoadSceneMode);
            }

            PushScene(entity);
        }

        public async void LoadSceneAsync(int sceneId, Action<float> loadingAction, Action loadEndAction)
        {
            if (loadingSceneId > 0)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneId);
                return;
            }

            SceneEntity entity = FindOrCreateSceneEntity(sceneId);

            if (!entity.Scene.isLoaded)
            {
                loadingSceneId = sceneId;
                this.loadingAction = loadingAction;
                var e = Game.UI.GetNavigationGroupEntity(UI.NavigationGroupDefine.Loading_Group);
                e.Show();
                await Game.Resource.LoadSceneAsync(entity.Path, entity.LoadSceneMode, null);

                float t = 500f;
                float i = 500f;
                while (i > 50f)
                {
                    i -= 1f;
                    LoadingHandler((t - i) / t);
                    await UniTask.Yield();
                }
                await UniTask.WaitForSeconds(0.2f);
                while (i > 0f)
                {
                    i -= 1f;
                    LoadingHandler((t - i) / t);
                    await UniTask.Yield();
                }
                await UniTask.Yield();
                e.Hide();
            }

            PushScene(entity);

            loadEndAction?.Invoke();

            this.loadingAction = null;
            loadingSceneId = -1;
        }

        private void LoadingHandler(float progress)
        {
            this.loadingAction?.Invoke(progress);
            Game.Event.Publish(new SceneLoadingProgress() { progress = progress });
        }

        private void PushScene(SceneEntity entity)
        {
            entity.SetActive(true);

            if (entity.LoadSceneMode == LoadSceneMode.Single)
            {
                activatedScenes.Clear();
            }
            else
            {
                if (activatedScenes.TryPeek(out SceneEntity s))
                {
                    s.SetActive(false);
                }
                SceneManager.SetActiveScene(entity.Scene);
            }

            activatedScenes.Push(entity);
        }

        private void PopScene()
        {
            if (activatedScenes.Count <= 1)
            {
                return;
            }

            if (activatedScenes.TryPop(out SceneEntity s1))
            {
                s1?.SetActive(false);
            }

            if (activatedScenes.TryPeek(out SceneEntity s2))
            {
                SceneManager.SetActiveScene(s2.Scene);
                s2.SetActive(true);
            }
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