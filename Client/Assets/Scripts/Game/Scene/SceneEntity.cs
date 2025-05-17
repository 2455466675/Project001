using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneEntity
    {
        public int SceneId => data.Id;
        public int BuildIndex => data.BuildIndex;
        public bool IsLoaded => Scene.isLoaded;
        public Scene Scene => data.LoadedScene;
        private string Path => data.Path;
        private LoadSceneMode LoadSceneMode => data.LoadSceneMode;

        private SceneData data;

        public void Init(SceneData data)
        {
            this.data = data;
        }

        public void SetActive(bool active)
        {
            GameObject[] objects = Scene.GetRootGameObjects();
            foreach (GameObject obj in objects)
            {
                obj.SetActive(active);
            }
        }

        public void LoadScene() 
        {
            if (IsLoaded) 
            {
                return;
            }
            SceneManager.LoadScene(Path, LoadSceneMode);
        }

        public async UniTask LoadSceneAsync(Action<float> action)
        {
            if (IsLoaded)
            {
                action?.Invoke(1f);
                return;
            }

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Path, LoadSceneMode);
            while (!asyncLoad.isDone)
            {
                action?.Invoke(asyncLoad.progress);
                await UniTask.Yield();
            }
        }

        public void UnloadScene() 
        {
            if (!IsLoaded) 
            {
                return;
            }
            SceneManager.UnloadSceneAsync(Path);
        }
    }
}
