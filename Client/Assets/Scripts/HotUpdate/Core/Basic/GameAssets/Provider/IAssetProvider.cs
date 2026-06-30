using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework.Core
{
    // 资源后端抽象入口：上层只依赖该接口与下面的句柄接口，
    // 更换底层资源框架（YooAsset / Addressables / 自研等）时只需替换一个 IAssetProvider 实现。
    public interface IAssetProvider
    {
        IAssetHandle LoadAssetSync(string assetPath);
        IAssetHandle LoadAssetAsync(string assetPath);

        ISubAssetsHandle LoadSubAssetsSync(string assetPath);
        ISubAssetsHandle LoadSubAssetsAsync(string assetPath);

        ISceneHandle LoadSceneSync(string assetPath, LoadSceneMode mode);
        UniTask<ISceneHandle> LoadSceneAsync(string assetPath, LoadSceneMode mode);

        UniTask UnloadUnusedAssetsAsync();
    }

    // 一次加载操作的抽象句柄：屏蔽各后端各自的 Handle 类型与状态枚举。
    public interface IOperationHandle
    {
        bool IsDone { get; }
        bool IsSucceed { get; }
        bool IsValid { get; }
        string LastError { get; }

        UniTask WaitAsync();
        void WaitForAsyncComplete();
        void Release();
    }

    public interface IAssetHandle : IOperationHandle
    {
        UnityEngine.Object AssetObject { get; }
        Type AssetType { get; }
    }

    public interface ISubAssetsHandle : IOperationHandle
    {
        T GetSubAssetObject<T>(string name) where T : UnityEngine.Object;
    }

    public interface ISceneHandle : IOperationHandle
    {
        Scene SceneObject { get; }
        void ActivateScene();
        UniTask UnloadAsync();
    }
}
