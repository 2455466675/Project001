using ECS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class SceneEntity : Entity
    {
        public Scene Scene { get; private set; }
        public int Index => Scene.buildIndex;
        public string Name => Scene.name;
        public string Path => Scene.path;
        
        public void SetActive(bool active)
        {
            GameObject[] objects = Scene.GetRootGameObjects();
            foreach (GameObject obj in objects)
            {
                obj.SetActive(active);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SceneComponent : Entity, IAwake
    {
        private Stack<SceneEntity> scenes;

        private string loadingSceneName;
        private Action<float> loadingAction;
        private bool isLoading;

        public void Awake()
        {
            scenes = new Stack<SceneEntity>();
        }

        public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (isLoading)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneName);
                return;
            }

            if (scenes.TryPeek(out SceneEntity e) && e.Name == sceneName) 
            {
                MLog.Warn($"要加载的场景已激活：{sceneName}");
                return;
            }

            Scene scene = GameWorld.Root.GetComponent<ResourceComponent>().LoadScene(sceneName, mode);

            

            //PushScene(sceneEntity, mode);
            
        }

        public async void LoadSceneAsync(string sceneName, Action<float> loadingAction, Action loadEndAction, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (isLoading)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneName);
                return;
            }

            if (scenes.TryPeek(out SceneEntity e) && e.Name == sceneName)
            {
                MLog.Warn($"要加载的场景已激活：{sceneName}");
                return;
            }

            isLoading = true;
            loadingSceneName = sceneName;
            this.loadingAction = loadingAction;

            Scene scene = await GameWorld.Root.GetComponent<ResourceComponent>().LoadSceneAsync(sceneName, mode, LoadingHandler);



            //PushScene(sceneEntity, mode);

            loadEndAction?.Invoke();

            loadingSceneName = string.Empty;
            this.loadingAction = null;
            isLoading = false;
        }

        private void LoadingHandler(float progress)
        {
            this.loadingAction?.Invoke(progress);
        }

        private void PushScene(SceneEntity scene, LoadSceneMode mode)
        {
            scene.SetActive(true);

            if (mode == LoadSceneMode.Single)
            {
                scenes.Clear();
            }
            else
            {
                if (scenes.TryPeek(out SceneEntity s))
                {
                    s.SetActive(false);
                }

                SceneManager.SetActiveScene(scene.Scene);
            }

            scenes.Push(scene);
        }

        private void PopScene()
        {
            if (scenes.Count <= 1)
            {
                return;
            }

            if (scenes.TryPop(out SceneEntity s1))
            {
                s1?.SetActive(false);
            }

            if (scenes.TryPeek(out SceneEntity s2))
            {
                SceneManager.SetActiveScene(s2.Scene);
                s2.SetActive(true);
            }
        }
    }
}
