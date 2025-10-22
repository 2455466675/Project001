using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using YooAsset;

namespace GameFramework.Core
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
            Visible,
            Unvisible,
        }

        public int SceneId => cfg.Id;
        public bool IsLoaded => handle != null && handle.SceneObject.isLoaded;
        public bool IsValid => handle != null && handle.SceneObject.IsValid();

        private readonly SceneConfig cfg;
        private SceneHandle handle;
        private State state;

        internal SceneEntity(SceneConfig cfg)
        {
            this.cfg = cfg;
        }

        internal void SetVisible(bool visible)
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
            GameObject[] objects = handle.SceneObject.GetRootGameObjects();
            foreach (GameObject obj in objects)
            {
                obj.SetActive(visible);
            }
            state = visible ? State.Visible : State.Unvisible;
        }

        internal void ActivateScene() 
        {
            if (handle == null) 
            {
                return;
            }
            handle.ActivateScene();
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
            var handle = Game.GetModule<AssetsManager>().LoadScene(cfg.Path, cfg.LoadSceneMode);
            this.handle = handle;
            state = State.Visible;
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
            this.handle = handle;
            state = State.Visible;
        }

        internal async UniTask UnloadAsync() 
        {
            if (handle == null) 
            {
                return;
            }
            await handle.UnloadAsync();
            handle = null;
        }
    }
}
