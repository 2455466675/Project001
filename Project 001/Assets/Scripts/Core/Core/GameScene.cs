using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
	public class GameScene : MonoBehaviour, ICore
    {
        public SceneMap CurrScene {get; private set;}

        public Transform characterContainer { get; private set; }

        private Coroutine loadSceneCo;
        private string loadingSceneName;
        private Action<AsyncOperation> loadingAction;
        private Action<SceneInfo> loadEndAction;

        public IEnumerator Init()
        {
            GameObject obj = new GameObject("characterContainer");
            GoHelper.DontDestroy(obj);
            characterContainer = obj.GetComponent<Transform>();
            yield return null;
        }

        public void LoadSceneAsync(string sceneName, LoadSceneMode mode, Action<AsyncOperation> loadingAction, Action<SceneInfo> loadEndAction)
        {
            if (loadSceneCo != null)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneName);
                return;
            }

            loadingSceneName = sceneName;
            this.loadingAction = loadingAction;
            this.loadEndAction = loadEndAction;

            loadSceneCo = StartCoroutine(GameCore.ResourceManager.LoadSceneAsync(sceneName, mode, LoadingHandler, LoadEndHandler));            
        }

        public void SetScene(SceneMap scene)
        {
            CurrScene = scene;
        }

        public SceneContainer GetContainer(string containerName)
        {
            if (CurrScene == null)
            {
                return null;
            }

            return CurrScene.GetContainer(containerName);
        }

        private void LoadingHandler(AsyncOperation operation)
        {
            loadingAction?.Invoke(operation);
        }

        private void LoadEndHandler(Scene scene)
        {
            SceneInfo sceneInfo = new();
            sceneInfo.SetScene(scene);
            loadEndAction?.Invoke(sceneInfo);

            loadingSceneName = string.Empty;
            loadingAction = null;
            loadEndAction = null;
            loadSceneCo = null;
        }
    }
}

