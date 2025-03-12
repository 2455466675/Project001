using EC;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class SceneEntity
    {
        public Scene Scene { get; private set; }
        public int Index => Scene.buildIndex;
        public string Name => Scene.name;
        public string Path => Scene.path;

        public SceneEntity(Scene scene) 
        {
            Scene = scene;                        
        }

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
    public class SceneComponent : EC.Component, IAwake
    {
        public Camera SceneCamera { get; private set; }
        public Transform CharacterContainer { get; private set; }

        private string loadingSceneName;
        private Action<float> loadingAction;
        private bool isLoading;

        private Stack<SceneEntity> sceneStack;

        private SceneEntity battleScene;

        public void Awake()
        {
            sceneStack = new Stack<SceneEntity>();

            SceneCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            CharacterContainer = GameObject.FindWithTag("CharacterContainer").transform;
        }

        public void EnterBattleScene(Action<SceneEntity> loadEndAction) 
        {
            if (battleScene == null)
            {
                LoadSceneAsync("BattleScene", LoadSceneMode.Additive, null, (s) => 
                {
                    battleScene = s;
                    loadEndAction?.Invoke(s);                    
                });
            }
            else
            {
                PushScene(battleScene, LoadSceneMode.Additive);
                loadEndAction?.Invoke(battleScene);
            }
            SceneCamera.transform.SetPositionAndRotation(new Vector3(10000, 0, -10), Quaternion.identity);
        }

        public void ExitBattleScene() 
        {
            PopScene();
            SceneCamera.transform.SetPositionAndRotation(new Vector3(0, 0, -10), Quaternion.identity);
        }
        
        public SceneEntity LoadScene(string sceneName, LoadSceneMode mode)
        {
            if (isLoading)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneName);
                return null;
            }

            Scene scene = GameWorld.Instance.GetComponent<ResourceComponent>().LoadScene(sceneName, mode);
            
            SceneEntity sceneEntity = new(scene);

            PushScene(sceneEntity, mode);

            return sceneEntity;
        }

        public async void LoadSceneAsync(string sceneName, LoadSceneMode mode, Action<float> loadingAction, Action<SceneEntity> loadEndAction)
        {
            if (isLoading)
            {
                MLog.Error("有一个正在加载中的场景:" + loadingSceneName);
                return;
            }

            isLoading = true;
            loadingSceneName = sceneName;
            this.loadingAction = loadingAction;

            Scene scene = await GameWorld.Instance.GetComponent<ResourceComponent>().LoadSceneAsync(sceneName, mode, LoadingHandler);

            SceneEntity sceneEntity = new(scene);

            PushScene(sceneEntity, mode);

            loadEndAction?.Invoke(sceneEntity);

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
                sceneStack.Clear();
            }
            else
            {
                if (sceneStack.TryPeek(out SceneEntity s))
                {
                    s.SetActive(false);
                }

                SceneManager.SetActiveScene(scene.Scene);
            }            

            sceneStack.Push(scene);
        }

        private void PopScene() 
        {
            if (sceneStack.Count <= 1) 
            {
                return;
            }

            if (sceneStack.TryPop(out SceneEntity s1)) 
            {
                s1?.SetActive(false);
            }

            if (sceneStack.TryPeek(out SceneEntity s2))
            {
                SceneManager.SetActiveScene(s2.Scene);
                s2.SetActive(true);
            }
        }
    }
}
