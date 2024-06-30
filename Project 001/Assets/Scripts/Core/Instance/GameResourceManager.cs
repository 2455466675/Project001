using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using YooAsset;
using System.Linq;

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

            // TODO ·ÖÆ½Ì¨
            var initParameters = new EditorSimulateModeParameters();
            var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
            initParameters.SimulateManifestFilePath = simulateManifestFilePath;
            yield return package.InitializeAsync(initParameters);

            YooAssets.SetDefaultPackage(package);
        }

        
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return package.LoadAssetSync<T>(path).AssetObject as T;
        }

        public IEnumerator LoadAssetAsync<T>(string path, Action<T> action) where T : UnityEngine.Object
        {
            AssetHandle handle = package.LoadAssetAsync<T>(path);
            yield return handle;
            action?.Invoke(handle.AssetObject as T);
        }

        public T[] LoadAllAssets<T>(string path) where T : UnityEngine.Object
        {
            AllAssetsHandle handle = package.LoadAllAssetsSync<T>(path);
            return handle.AllAssetObjects.Cast<T>().ToArray();
        }

        public SceneInfo LoadScene(string sceneName, LoadSceneMode mode)
        {
            Scene scene = SceneManager.LoadScene(sceneName, new LoadSceneParameters(mode));
            SceneInfo sceneInfo = new SceneInfo();
            sceneInfo.SetScene(scene);
            return sceneInfo;
        }

        public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode, Action<AsyncOperation> action, Action<SceneInfo> loadEndEvt)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
            while (!asyncLoad.isDone)
            {
                action?.Invoke(asyncLoad);
                yield return null;
            }

            SceneInfo sceneInfo = new SceneInfo();
            sceneInfo.SetScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
            loadEndEvt?.Invoke(sceneInfo);
        }

        public Sprite GetSprite(string spriteName)
        {
            MLog.Log($"spriteName:{spriteName}");
            return null;
        }
    }
}