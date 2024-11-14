using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using YooAsset;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Game.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class GameResourceManager : MonoBehaviour, ICore
    {
        private ResourcePackage package;

        public IEnumerator Init()
        {      
            YooAssets.Initialize();

            package = YooAssets.TryGetPackage("DefaultPackage");
            package ??= YooAssets.CreatePackage("DefaultPackage");

            YooAssets.SetDefaultPackage(package);

            RuntimePlatform platform = Application.platform;

            if (platform == RuntimePlatform.WindowsEditor)
            {
                EditorSimulateModeParameters parameters = new EditorSimulateModeParameters();
                var smfp = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
                parameters.SimulateManifestFilePath = smfp;

                yield return package.InitializeAsync(parameters);
            }
            else if(platform == RuntimePlatform.WindowsPlayer)
            {
                OfflinePlayModeParameters parameters = new OfflinePlayModeParameters();
                yield return package.InitializeAsync(parameters);
            }
      
            yield return null;
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

        public SceneInfo LoadScene(string sceneName, LoadSceneMode mode)
        {
            Scene scene = SceneManager.LoadScene(sceneName, new LoadSceneParameters(mode));
            SceneInfo sceneInfo = new();
            sceneInfo.SetScene(scene);
            return sceneInfo;
        }

        public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode, Action<AsyncOperation> action, Action<Scene> loadEndEvt)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
            while (!asyncLoad.isDone)
            {
                action?.Invoke(asyncLoad);
                yield return null;
            }
            loadEndEvt?.Invoke(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        }

        public Sprite GetSprite(string spriteName)
        {
            MLog.Log($"spriteName:{spriteName}");
            return null;
        }
    }
}