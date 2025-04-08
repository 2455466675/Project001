using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceComponent : ECS.Entity
    {
        private ResourcePackage package;

        public async UniTask Init(GameInitConfig initCfg)
        {
            string packageName = initCfg.PackageName;

            YooAssets.Initialize();

            package = YooAssets.TryGetPackage(packageName);
            package ??= YooAssets.CreatePackage(packageName);

            YooAssets.SetDefaultPackage(package);

            RuntimePlatform platform = Application.platform;

            if (platform == RuntimePlatform.WindowsEditor)
            {
                EditorSimulateModeParameters parameters = new EditorSimulateModeParameters();
                var smfp = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, packageName);
                parameters.SimulateManifestFilePath = smfp;

                await package.InitializeAsync(parameters);
            }
            else if (platform == RuntimePlatform.WindowsPlayer)
            {
                OfflinePlayModeParameters parameters = new OfflinePlayModeParameters();
                await package.InitializeAsync(parameters);
            }
        }

        /// <summary>
        /// 从Resources下加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadFormRes<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 加载资源（Assets下的路径）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return package.LoadAssetSync<T>(path).AssetObject as T;
        }

        /// <summary>
        /// 异步加载资源（Assets下的路径）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            AssetHandle handle = package.LoadAssetAsync<T>(path);
            await handle;
            return handle.GetAssetObject<T>();
        }

        public T[] LoadAllAssets<T>(string path) where T : UnityEngine.Object
        {
            AllAssetsHandle handle = package.LoadAllAssetsSync<T>(path);
            return handle.AllAssetObjects.Cast<T>().ToArray();
        }

        /// <summary>
        /// 加载并实例化一个游戏物体（Assets下的路径）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject LoadAndInstantiate(string path, Transform parent)
        {
            GameObject obj = LoadAsset<GameObject>(path);
            return GoHelper.Instantiate(obj, parent);
        }

        public Scene LoadScene(string sceneName, LoadSceneMode mode)
        {
            SceneManager.LoadScene(sceneName, mode);
            return SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        }

        public async UniTask<Scene> LoadSceneAsync(string sceneName, LoadSceneMode mode, Action<float> action)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
            while (!asyncLoad.isDone)
            {
                action?.Invoke(asyncLoad.progress);
                await UniTask.Yield();
            }
            return SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        }

        public Sprite GetSprite(string spriteName)
        {            
            return null;
        }
    }
}
