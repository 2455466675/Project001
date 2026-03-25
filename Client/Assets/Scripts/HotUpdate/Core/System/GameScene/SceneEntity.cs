using UnityEngine;
using YooAsset;
using Cysharp.Threading.Tasks;

namespace GameFramework.Core
{
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
                MDebug.Log("!IsLoaded", SceneId);
                return;
            }
            if (!IsValid)
            {
                MDebug.Log("!IsValid", SceneId);
                return;
            }

            if (state == State.None || state == State.Loading)
            {
                MDebug.Log("state == State.None || state == State.Loading", SceneId);
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
            var handle = Game.Resources.LoadScene(cfg.Path, cfg.LoadSceneMode);
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

            var handle = Game.Resources.LoadScentAsync(cfg.Path, cfg.LoadSceneMode);
            await handle;

            this.handle = handle;
            state = State.Visible;
        }

        internal async UniTask UnloadAsync()
        {
            if (handle == null)
            {
                return;
            }
            if (handle.IsValid)
            {
                await handle.UnloadAsync();
            }
            handle = null;
        }
    }
}