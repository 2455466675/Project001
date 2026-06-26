using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            await UniTask.CompletedTask;
        }

        public T LoadFromResources<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        #region Raw

        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            AssetItem assetItem = CreateAssetItem<T>(path);
            if (assetItem == null)
            {
                return null;
            }
            T asset = assetItem.GetAsset<T>();
            ActiveAsset(asset, path);
            return asset;
        }

        public async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            AssetItem assetItem = await CreateAssetItemAsync<T>(path);
            if (assetItem == null)
            {
                return null;
            }
            T asset = assetItem.GetAsset<T>();
            ActiveAsset(asset, path);
            return asset;
        }

        #endregion

        #region Instantiate

        public GameObject Instantiate(string path)
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);          
            if (assetItem == null)
            {
                return null;
            }
            GameObject asset = assetItem.Instantiate();
            ActiveAsset(asset, path);
            return asset;
        }

        public GameObject Instantiate(string path, Transform parent)
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);
            if (assetItem == null)
            {
                return null;
            }
            GameObject asset = assetItem.Instantiate(parent);
            ActiveAsset(asset, path);
            return asset;
        }

        public GameObject Instantiate(string path, Transform parent, bool worldPositionStays)
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);
            if (assetItem == null)
            {
                return null;
            }
            GameObject asset = assetItem.Instantiate(parent, worldPositionStays);
            ActiveAsset(asset, path);
            return asset;
        }

        public GameObject Instantiate(string path, Vector3 position, Quaternion rotation)
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);
            if (assetItem == null)
            {
                return null;
            }
            GameObject asset = assetItem.Instantiate(position, rotation);
            ActiveAsset(asset, path);
            return asset;
        }

        public GameObject Instantiate(string path, Vector3 position, Quaternion rotation, Transform parent)
        {
            AssetItem assetItem = CreateAssetItem<GameObject>(path);
            if (assetItem == null)
            {
                return null;
            }
            GameObject asset = assetItem.Instantiate(position, rotation, parent);
            ActiveAsset(asset, path);
            return asset;
        }

        public async UniTask<GameObject> InstantiateAsync(string path, Transform parent)
        {
            AssetItem assetItem = await CreateAssetItemAsync<GameObject>(path);
            if (assetItem == null)
            {
                return null;
            }
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
            
            if (assetItem.ReleaseAsset(asset))
            {
                activeAssets.Remove(instanceID);    
            }

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

            // 已有异步加载在途：接管其句柄并强制同步完成，避免重复创建 handle 造成泄漏。
            // 该句柄归异步流程所有，其失败释放与登记清理统一交由异步侧处理，这里不碰，防止双重 Release。
            if (requesters.TryGetValue(path, out AssetRequester requester))
            {
                AssetHandle pending = requester.Handle;
                pending.WaitForAsyncComplete();

                if (pending.Status != EOperationStatus.Succeed)
                {
                    return null;
                }

                assetItem = new AssetItem(pending);
                assets[path] = assetItem;
                return assetItem;
            }

            AssetHandle handle = YooAssets.LoadAssetSync<T>(path);
            if (handle.Status != EOperationStatus.Succeed)
            {
                Debug.LogError($"[GameAssetsManager] 同步加载资源失败: {path}, {handle.LastError}");
                handle.Release();
                return null;
            }

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
                assets.TryGetValue(path, out assetItem);
                return assetItem;
            }

            requester = new AssetRequester();
            requesters[path] = requester;

            AssetHandle handle = YooAssets.LoadAssetAsync<T>(path);
            requester.Handle = handle; // 登记在途句柄，使同步路径可接管并强制完成

            try
            {
                await handle;

                // 同步加载可能在 await 期间已接管同一 handle 并落表，此时直接复用，避免重复包装与重复释放
                if (assets.TryGetValue(path, out AssetItem existing))
                {
                    return existing;
                }

                if (handle.Status != EOperationStatus.Succeed)
                {
                    Debug.LogError($"[GameAssetsManager] 异步加载资源失败: {path}, {handle.LastError}");
                    handle.Release();
                    return null;
                }

                assetItem = new AssetItem(handle);
                assets[path] = assetItem;
                return assetItem;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[GameAssetsManager] 异步加载资源异常: {path}\n{e}");
                handle.Release();
                return null;
            }
            finally
            {
                requesters.Remove(path);
                requester.SetResult(handle);
            }
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
