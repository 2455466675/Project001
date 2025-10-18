using Cysharp.Threading.Tasks;
using GameFramework.Core;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    /// <summary>
    /// 
    /// </summary>
    internal class SceneEntity
    {
        private enum State 
        {
            None = 0,
            Loading,
            Active,
            Deactive,
        }

        public int SceneId => cfg.Id;
        public bool IsLoaded => scene.isLoaded;
        public bool IsValid => scene.IsValid();

        private readonly SceneConfig cfg;
        private State state;
        private Scene scene;

        internal SceneEntity(SceneConfig cfg)
        {
            this.cfg = cfg;
        }

        internal void SetActive(bool active)
        {
            if (!IsLoaded)
            {
                return;
            }
            if (!IsValid) 
            {
                return;
            }
           
            if (state == State.None || state == State.Loading)
            {
                return;
            }
            GameObject[] objects = scene.GetRootGameObjects();
            foreach (GameObject obj in objects)
            {
                obj.SetActive(active);
            }
            state = active ? State.Active : State.Deactive;
        }

        internal void Load() 
        {
            if (IsLoaded) 
            {
                return;
            }
            if (state == State.Loading) 
            {
                return;
            }
            state = State.Loading;
            scene = Game.GetModule<AssetsManager>().LoadScene(cfg.Path, cfg.LoadSceneMode);
            state = State.Active;
        }

        internal async UniTask LoadAsync()
        {
            if (IsLoaded)
            {
                return;
            }
            if (state == State.Loading)
            {
                return;
            }
            state = State.Loading;
            var handle = Game.GetModule<AssetsManager>().LoadScentAsync(cfg.Path, cfg.LoadSceneMode);
            while (!handle.IsDone)
            {
                await UniTask.Yield();
            }
            scene = handle.SceneObject;
            state = State.Active;
        }

        internal async UniTask UnloadAsync() 
        {
            if (!IsLoaded) 
            {
                return;
            }
            await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
        }
    }
}
