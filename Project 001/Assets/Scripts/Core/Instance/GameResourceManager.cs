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
      
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return package.LoadAssetSync<T>(path).AssetObject as T;
        }

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