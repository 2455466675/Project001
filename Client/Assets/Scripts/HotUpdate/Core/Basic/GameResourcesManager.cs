using System.Collections.Generic;
using System.Linq;
using YooAsset;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using GameFramework.Utility;

namespace GameFramework.Core
{
    public class GameResourcesManager
    {
        private const string DefaultPackageName = "DefaultPackage";

        // 缓存已加载资源的句柄并按引用计数管理。YooAsset 的句柄是引用计数的，
        // 加载后立即 Release 会让资源在 UnloadUnusedAssets 时被误卸载，故改为缓存持有、归还后再释放。
        private class AssetRecord
        {
            public AssetHandle Handle;
            public int RefCount;
            public bool Permanent;
        }

        private readonly Dictionary<string, AssetRecord> assetCache = new Dictionary<string, AssetRecord>();
        // 记录进行中的异步加载，合并对同一路径的并发请求，避免重复加载产生游离句柄
        private readonly Dictionary<string, UniTaskCompletionSource<AssetHandle>> loadingTasks = new Dictionary<string, UniTaskCompletionSource<AssetHandle>>();

        //private SpriteManager spriteManager;

        internal GameResourcesManager()
        {
            //spriteManager = new SpriteManager();
        }

        public async UniTask Init()
        {

        }

        /// <summary>
        /// 从 Resources 目录加载（Unity 内置管线，不参与 YooAsset 的句柄管理）
        /// </summary>
        public T LoadFormResources<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 同步加载资源（Assets 下的路径），计入引用计数。用完须调用 ReleaseAsset 归还。
        /// </summary>
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            AssetRecord record = GetOrCreateRecord<T>(path);
            record.RefCount++;
            return record.Handle.GetAssetObject<T>();
        }

        /// <summary>
        /// 异步加载资源（Assets 下的路径），计入引用计数。对同一路径的并发请求会合并为一次底层加载。
        /// </summary>
        public async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            AssetRecord record = await GetOrCreateRecordAsync<T>(path);
            record.RefCount++;
            return record.Handle.GetAssetObject<T>();
        }

        /// <summary>
        /// 加载常驻资源（如配置表）。常驻资源不参与引用计数与卸载，全局存活，避免被 UnloadUnusedAssets 误伤。
        /// </summary>
        public T LoadPermanent<T>(string path) where T : UnityEngine.Object
        {
            AssetRecord record = GetOrCreateRecord<T>(path);
            record.Permanent = true;
            return record.Handle.GetAssetObject<T>();
        }

        /// <summary>
        /// 归还一个由 LoadAsset/LoadAssetAsync 加载的资源。引用归零且非常驻时才真正释放句柄。
        /// </summary>
        public void ReleaseAsset(string path)
        {
            if (!assetCache.TryGetValue(path, out AssetRecord record))
            {
                return;
            }
            if (record.Permanent)
            {
                return;
            }
            record.RefCount--;
            if (record.RefCount <= 0)
            {
                record.Handle.Release();
                assetCache.Remove(path);
            }
        }

        public T[] LoadAllAssets<T>(string path) where T : UnityEngine.Object
        {
            var handle = YooAssets.LoadAllAssetsSync<T>(path);
            T[] result = handle.AllAssetObjects.Cast<T>().ToArray();
            handle.Release();
            return result;
        }

        public SceneHandle LoadScene(string path, LoadSceneMode mode)
        {
            var handle = YooAssets.LoadSceneSync(path, mode);
            return handle;
        }

        public SceneHandle LoadScentAsync(string path, LoadSceneMode mode)
        {
            var handle = YooAssets.LoadSceneAsync(path, mode);
            return handle;
        }

        /// <summary>
        /// 加载并实例化一个游戏物体（Assets 下的路径）
        /// </summary>
        public GameObject LoadAndInstantiate(string path, Transform parent)
        {
            GameObject obj = LoadAsset<GameObject>(path);
            return GoHelper.Instantiate(obj, parent);
        }

        public async UniTask<GameObject> LoadAndInstantiateAsync(string path, Transform parent)
        {
            GameObject obj = await LoadAssetAsync<GameObject>(path);
            return GoHelper.Instantiate(obj, parent);
        }

        public async UniTask UnloadUnusedAssetsAsync()
        {
            var package = YooAssets.GetPackage(DefaultPackageName);
            await package.UnloadUnusedAssetsAsync();
        }

        public Sprite GetSprite(string spriteName)
        {
            //return spriteManager.GetSprite(spriteName);
            return null;
        }

        private AssetRecord GetOrCreateRecord<T>(string path) where T : UnityEngine.Object
        {
            if (assetCache.TryGetValue(path, out AssetRecord record))
            {
                return record;
            }

            AssetHandle handle = YooAssets.LoadAssetSync<T>(path);
            record = new AssetRecord { Handle = handle, RefCount = 0 };
            assetCache[path] = record;
            return record;
        }

        private async UniTask<AssetRecord> GetOrCreateRecordAsync<T>(string path) where T : UnityEngine.Object
        {
            if (assetCache.TryGetValue(path, out AssetRecord record))
            {
                return record;
            }

            // 已有同路径加载在途，等待其完成后直接复用缓存，避免重复加载
            if (loadingTasks.TryGetValue(path, out UniTaskCompletionSource<AssetHandle> pending))
            {
                await pending.Task;
                return assetCache[path];
            }

            var source = new UniTaskCompletionSource<AssetHandle>();
            loadingTasks[path] = source;

            AssetHandle handle = YooAssets.LoadAssetAsync<T>(path);
            await handle;

            record = new AssetRecord { Handle = handle, RefCount = 0 };
            assetCache[path] = record;

            // 先建好缓存记录再唤醒等待者，确保等待者醒来即可命中缓存
            loadingTasks.Remove(path);
            source.TrySetResult(handle);

            return record;
        }
    }
}
