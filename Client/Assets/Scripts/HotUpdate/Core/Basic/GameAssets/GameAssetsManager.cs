using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using YooAsset;

namespace GameFramework.Core
{
    public class GameAssetsManager
    {
        private const string DefaultPackageName = "DefaultPackage";

        private Dictionary<string, AssetItem> assets;
        private Dictionary<string, AssetRequester> requesters;
        private Dictionary<int, string> activeAssets;

        internal GameAssetsManager()
        {
            assets = new Dictionary<string, AssetItem>();
            requesters = new Dictionary<string, AssetRequester>();
            activeAssets = new Dictionary<int, string>();
        }

        public async UniTask Init()
        {

        }

        public T LoadFormResources<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        #region Raw

        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);
            T asset = assetItem.GetAsset<T>();
            ActiveAsset(asset, path);
            return asset;
        }

        public async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            AssetItem assetItem = await CreateAssetItemAsync<T>(path);
            T asset = assetItem.GetAsset<T>();
            ActiveAsset(asset, path);
            return asset;
        }

        #endregion

        #region Instantiate

        public T Instantiate<T>(string path) where T : UnityEngine.Object
        {
            AssetItem assetItem = CreateAssetItem<T>(path);
            T asset = assetItem.Instantiate<T>();
            ActiveAsset(asset, path);
            return asset;
        }

        public GameObject Instantiate(string path)
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);          
            GameObject asset = assetItem.Instantiate();
            ActiveAsset(asset, path);
            return asset;
        }

        public GameObject Instantiate(string path, Transform parent)
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);
            GameObject asset = assetItem.Instantiate(parent);
            ActiveAsset(asset, path);
            return asset;
        }

        public GameObject Instantiate(string path, Transform parent, bool worldPositionStays)
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);
            GameObject asset = assetItem.Instantiate(parent, worldPositionStays);
            ActiveAsset(asset, path);
            return asset;
        }

        public GameObject Instantiate(string path, Vector3 position, Quaternion rotation)
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);
            GameObject asset = assetItem.Instantiate(position, rotation);
            ActiveAsset(asset, path);
            return asset;
        }

        public GameObject Instantiate(string path, Vector3 position, Quaternion rotation, Transform parent)
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);
            GameObject asset = assetItem.Instantiate(position, rotation, parent);
            ActiveAsset(asset, path);
            return asset;
        }

        public async UniTask<GameObject> InstantiateAsync(string path, Transform parent)
        {
            AssetItem assetItem = await CreateAssetItemAsync<GameObject>(path);
            GameObject asset = assetItem.Instantiate(parent);
            ActiveAsset(asset, path);
            return asset;
        }

        #endregion

        #region Scene

        public SceneHandle LoadScene(string path, LoadSceneMode mode)
        {
            var handle = YooAssets.LoadSceneSync(path, mode);
            return handle;
        }

        public SceneHandle LoadSceneAsync(string path, LoadSceneMode mode)
        {
            var handle = YooAssets.LoadSceneAsync(path, mode);
            return handle;
        }

        #endregion

        public void ReleaseAsset(UnityEngine.Object asset)
        {
            if (asset == null)
            {
                return;
            }

            int instanceID = asset.GetInstanceID();
            if (!activeAssets.TryGetValue(instanceID, out var assetPath))
            {
                return;
            }

            if (!assets.TryGetValue(assetPath, out var assetItem))
            {
                return;
            }

            activeAssets.Remove(instanceID);
            assetItem.ReleaseAsset(asset);

            if (assetItem.RefCount <= 0)
            {
                assetItem.Release();
                assets.Remove(assetPath);
            }
        }

        public async UniTask UnloadUnusedAssetsAsync()
        {
            var package = YooAssets.GetPackage(DefaultPackageName);
            await package.UnloadUnusedAssetsAsync();
        }

        private AssetItem CreateAssetItem<T>(string path) where T : UnityEngine.Object
        {
            if (assets.TryGetValue(path, out AssetItem assetItem))
            {
                return assetItem;
            }

            AssetHandle handle = YooAssets.LoadAssetSync<T>(path);
            assetItem = new AssetItem(handle);
            assets[path] = assetItem;
            return assetItem;
        }

        private async UniTask<AssetItem> CreateAssetItemAsync<T>(string path) where T : UnityEngine.Object
        {
            if (assets.TryGetValue(path, out AssetItem assetItem))
            {
                return assetItem;
            }

            if (requesters.TryGetValue(path, out AssetRequester requester))
            {
                await requester.Task;
                return assets[path];
            }

            requester = new AssetRequester();
            requesters[path] = requester;

            AssetHandle handle = YooAssets.LoadAssetAsync<T>(path);
            await handle;

            assetItem = new AssetItem(handle);
            assets[path] = assetItem;

            requesters.Remove(path);
            requester.SetResult(handle);

            return assetItem;
        }

        private void ActiveAsset(UnityEngine.Object asset, string assetPath)
        {
            if (asset == null)
            {
                return;
            }
            activeAssets[asset.GetInstanceID()] = assetPath;
        }
    }
}
