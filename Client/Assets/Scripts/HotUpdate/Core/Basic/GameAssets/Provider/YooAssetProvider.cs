using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace GameFramework.Core
{
    // YooAsset 后端实现：所有对 YooAsset 类型（ResourcePackage/AssetHandle/SubAssetsHandle/SceneHandle/EOperationStatus 等）的依赖
    // 都收敛在本文件内，上层代码不再直接引用 YooAsset。
    public class YooAssetProvider : IAssetProvider
    {
        private readonly string packageName;
        private ResourcePackage package;

        public YooAssetProvider(string packageName)
        {
            this.packageName = packageName;
        }

        // 显式持有指定资源包并据此加载/卸载，而非依赖 YooAsset 的全局默认包，
        // 这样多包并存时行为明确；首次使用时再绑定，规避 Provider 构造早于包初始化的时序问题。
        private ResourcePackage Package
        {
            get
            {
                if (package == null)
                {
                    package = YooAssets.GetPackage(packageName);
                }
                return package;
            }
        }

        public IAssetHandle LoadAssetSync(string assetPath)
        {
            return new YooAssetHandle(Package.LoadAssetSync(assetPath));
        }

        public IAssetHandle LoadAssetAsync(string assetPath)
        {
            return new YooAssetHandle(Package.LoadAssetAsync(assetPath));
        }

        public ISubAssetsHandle LoadSubAssetsSync(string assetPath)
        {
            return new YooSubAssetsHandle(Package.LoadSubAssetsSync(assetPath));
        }

        public ISubAssetsHandle LoadSubAssetsAsync(string assetPath)
        {
            return new YooSubAssetsHandle(Package.LoadSubAssetsAsync(assetPath));
        }

        public ISceneHandle LoadSceneSync(string assetPath, LoadSceneMode mode)
        {
            return new YooSceneHandle(Package.LoadSceneSync(assetPath, mode));
        }

        public async UniTask<ISceneHandle> LoadSceneAsync(string assetPath, LoadSceneMode mode)
        {
            var handle = Package.LoadSceneAsync(assetPath, mode);
            await handle;
            return new YooSceneHandle(handle);
        }

        public async UniTask UnloadUnusedAssetsAsync()
        {
            await Package.UnloadUnusedAssetsAsync();
        }
    }

    internal class YooAssetHandle : IAssetHandle
    {
        private AssetHandle handle;

        public YooAssetHandle(AssetHandle handle)
        {
            this.handle = handle;
        }

        public bool IsDone => handle != null && handle.IsValid && handle.IsDone;
        public bool IsSucceed => handle != null && handle.Status == EOperationStatus.Succeed;
        public bool IsValid => handle != null && handle.IsValid;
        public string LastError => handle == null ? string.Empty : handle.LastError;

        public UnityEngine.Object AssetObject => handle == null ? null : handle.AssetObject;
        public Type AssetType => handle?.GetAssetInfo()?.AssetType;

        public async UniTask WaitAsync()
        {
            var h = handle;
            if (h != null)
            {
                await h;
            }
        }

        public void WaitForAsyncComplete()
        {
            handle?.WaitForAsyncComplete();
        }

        public void Release()
        {
            if (handle != null)
            {
                handle.Release();
                handle = null;
            }
        }
    }

    internal class YooSubAssetsHandle : ISubAssetsHandle
    {
        private SubAssetsHandle handle;

        public YooSubAssetsHandle(SubAssetsHandle handle)
        {
            this.handle = handle;
        }

        public bool IsDone => handle != null && handle.IsValid && handle.IsDone;
        public bool IsSucceed => handle != null && handle.Status == EOperationStatus.Succeed;
        public bool IsValid => handle != null && handle.IsValid;
        public string LastError => handle == null ? string.Empty : handle.LastError;

        public T GetSubAssetObject<T>(string name) where T : UnityEngine.Object
        {
            return handle == null ? null : handle.GetSubAssetObject<T>(name);
        }

        public async UniTask WaitAsync()
        {
            var h = handle;
            if (h != null)
            {
                await h;
            }
        }

        public void WaitForAsyncComplete()
        {
            handle?.WaitForAsyncComplete();
        }

        public void Release()
        {
            if (handle != null)
            {
                handle.Release();
                handle = null;
            }
        }
    }

    internal class YooSceneHandle : ISceneHandle
    {
        private SceneHandle handle;

        public YooSceneHandle(SceneHandle handle)
        {
            this.handle = handle;
        }

        public bool IsDone => handle != null && handle.IsDone;
        public bool IsSucceed => handle != null && handle.Status == EOperationStatus.Succeed;
        public bool IsValid => handle != null && handle.IsValid;
        public string LastError => handle == null ? string.Empty : handle.LastError;

        public Scene SceneObject => handle == null ? default : handle.SceneObject;

        public void ActivateScene()
        {
            handle?.ActivateScene();
        }

        public async UniTask WaitAsync()
        {
            var h = handle;
            if (h != null)
            {
                await h;
            }
        }

        public void WaitForAsyncComplete()
        {
            // YooAsset 场景句柄不支持强制同步完成，场景加载只走异步路径。
        }

        public async UniTask UnloadAsync()
        {
            if (handle != null && handle.IsValid)
            {
                await handle.UnloadAsync();
            }
            handle = null;
        }

        // 场景资源由 UnloadAsync 负责回收，Release 不单独处理。
        public void Release()
        {
        }
    }
}
