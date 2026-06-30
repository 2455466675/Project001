using Config;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework.Core
{
    public enum SceneMode 
    {
        Basic = 0,
        Single = 1,
        Overlay = 2,
    }


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
        public SceneMode SceneMode { get; private set; }
        public bool IsLoaded => handle != null && handle.SceneObject.isLoaded;
        public bool IsValid => handle != null && handle.SceneObject.IsValid();

        private readonly SceneCfg cfg;
        private ISceneHandle handle;
        private State state;
        private LoadSceneMode loadSceneMode;

        internal SceneEntity(SceneCfg cfg)
        {
            this.cfg = cfg;
            this.SceneMode = (SceneMode)cfg.LoadMode;
            loadSceneMode = this.SceneMode == SceneMode.Basic ? LoadSceneMode.Single : LoadSceneMode.Additive;
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
                MDebug.Warn($"{SceneId} : 场景尚未加载完成。state = {state}");
                return;
            }

            if (SceneMode != SceneMode.Single)
            {
                MDebug.Warn($"{SceneId} : 此场景不允许隐藏。如果是Overlay场景，请选择卸载它");
                return;
            }

            GameObject[] objects = handle.SceneObject.GetRootGameObjects();
            foreach (GameObject obj in objects)
            {
                obj.SetActive(visible);
            }
            state = visible ? State.Visible : State.Unvisible;
        }

        /// <summary>
        /// 设置当前场景为激活场景
        /// </summary>
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
            var handle = Game.Assets.LoadScene(cfg.Path, loadSceneMode);
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

            var handle = await Game.Assets.LoadSceneAsync(cfg.Path, loadSceneMode);
            if (handle == null || !handle.IsSucceed)
            {
                MDebug.Log($"场景加载失败 : {cfg.Path}");
                state = State.None;
                return;
            }

            this.handle = handle;
            state = State.Visible;
        }

        internal async UniTask UnloadAsync()
        {
            if (state == State.Loading)
            {
                return;
            }            
            if (handle != null && handle.IsValid)
            {
                await handle.UnloadAsync();
            }
            handle = null;
            state = State.None;
        }
    }
}