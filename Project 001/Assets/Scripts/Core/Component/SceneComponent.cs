using System;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class SceneInfo
    {
        public int Index { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public Scene Scene { get; private set; }
        public void SetScene(Scene scene)
        {
            Scene = scene;
            Index = scene.buildIndex;
            Name = scene.name;
            Path = scene.path;            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SceneComponent : EC.Component
    {
        private string loadingSceneName;
        private Action<float> loadingAction;
        private Action<SceneInfo> loadEndAction;

        private bool isLoading;

        public void LoadScene(string sceneName, LoadSceneMode mode)
        {
            if (isLoading)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneName);
                return;
            }

            isLoading = true;
            loadingSceneName = sceneName;

            GameWorld.Instance.GetComponent<ResourceComponent>().LoadScene(sceneName, mode);

            isLoading = false;
            loadingSceneName = string.Empty;
        }

        public async void LoadSceneAsync(string sceneName, LoadSceneMode mode, Action<float> loadingAction, Action<SceneInfo> loadEndAction)
        {
            if (isLoading)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneName);
                return;
            }

            isLoading = true;
            loadingSceneName = sceneName;
            this.loadingAction = loadingAction;
            this.loadEndAction = loadEndAction;

            await GameWorld.Instance.GetComponent<ResourceComponent>().LoadSceneAsync(sceneName, mode, LoadingHandler, LoadEndHandler);
        }

        private void LoadingHandler(float progress)
        {
            loadingAction?.Invoke(progress);
        }

        private void LoadEndHandler(Scene scene)
        {
            SceneInfo sceneInfo = new();
            sceneInfo.SetScene(scene);
            loadEndAction?.Invoke(sceneInfo);
            
            loadingSceneName = string.Empty;
            loadingAction = null;
            loadEndAction = null;
            isLoading = false;
        }
    }
}
