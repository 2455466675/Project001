using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework.Core
{
    public class GameAssetsManager
    {
        // 资源管理器接口
        private readonly IAssetProvider provider;

        private readonly Dictionary<string, AssetEntry> assetEntries;   //已加载完成的资源
        private readonly Dictionary<string, AssetEntry> loadingEntries; //正在加载中的资源
        private readonly Dictionary<int, string> activeEntries; //活跃资源（InstanceID => AssetPath）
        private readonly List<string> unloadBuffer;

        private readonly SpritePathProxy spritePathProxy;

        internal GameAssetsManager(IAssetProvider provider)
        {
            this.provider = provider;
            assetEntries = new Dictionary<string, AssetEntry>();
            loadingEntries = new Dictionary<string, AssetEntry>();
            activeEntries = new Dictionary<int, string>();
            unloadBuffer = new List<string>();

            spritePathProxy = new SpritePathProxy();
        }

        public async UniTask Init()
        {
            await spritePathProxy.Init();            
        }

        public T LoadFromResources<T>(string assetPath) where T : UnityEngine.Object
        {
            return Resources.Load<T>(assetPath);
        }

        #region Raw

        public T LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            var entry = CreateAssetEntry<RawAssetEntry>(assetPath);
            if (entry == null)
            {
                return default;
            }
            T asset = entry.GetAsset<T>();
            ActiveAsset(asset, assetPath);
            return asset;
        }

        public async UniTask<T> LoadAssetAsync<T>(string assetPath) where T : UnityEngine.Object
        {
            var entry = await CreateAssetEntryAsync<RawAssetEntry>(assetPath);
            if (entry == null)
            {
                return default;
            }
            T asset = entry.GetAsset<T>();
            ActiveAsset(asset, assetPath);
            return asset;
        }

        #endregion

        #region Prefab

        public GameObject Instantiate(string assetPath)
        {
            var entry = CreateAssetEntry<PrefabAssetEntry>(assetPath);          
            if (entry == null)
            {
                return null;
            }
            GameObject asset = entry.Instantiate();
            ActiveAsset(asset, assetPath);
            return asset;
        }

        public GameObject Instantiate(string assetPath, Transform parent)
        {
            var entry = CreateAssetEntry<PrefabAssetEntry>(assetPath);
            if (entry == null)
            {
                return null;
            }
            GameObject asset = entry.Instantiate(parent);
            ActiveAsset(asset, assetPath);
            return asset;
        }

        public GameObject Instantiate(string assetPath, Transform parent, bool worldPositionStays)
        {
            var entry = CreateAssetEntry<PrefabAssetEntry>(assetPath);
            if (entry == null)
            {
                return null;
            }
            GameObject asset = entry.Instantiate(parent, worldPositionStays);
            ActiveAsset(asset, assetPath);
            return asset;
        }

        public GameObject Instantiate(string assetPath, Vector3 position, Quaternion rotation)
        {
            var entry = CreateAssetEntry<PrefabAssetEntry>(assetPath);
            if (entry == null)
            {
                return null;
            }
            GameObject asset = entry.Instantiate(position, rotation);
            ActiveAsset(asset, assetPath);
            return asset;
        }

        public GameObject Instantiate(string assetPath, Vector3 position, Quaternion rotation, Transform parent)
        {
            var entry = CreateAssetEntry<PrefabAssetEntry>(assetPath);
            if (entry == null)
            {
                return null;
            }
            GameObject asset = entry.Instantiate(position, rotation, parent);
            ActiveAsset(asset, assetPath);
            return asset;
        }

        public async UniTask<GameObject> InstantiateAsync(string assetPath, Transform parent)
        {
            var entry = await CreateAssetEntryAsync<PrefabAssetEntry>(assetPath);
            if (entry == null)
            {
                return null;
            }
            GameObject asset = entry.Instantiate(parent);
            ActiveAsset(asset, assetPath);
            return asset;
        }

        #endregion

        #region Scene

        public ISceneHandle LoadScene(string assetPath, LoadSceneMode mode)
        {
            return provider.LoadSceneSync(assetPath, mode);
        }

        public async UniTask<ISceneHandle> LoadSceneAsync(string assetPath, LoadSceneMode mode)
        {
            return await provider.LoadSceneAsync(assetPath, mode);
        }

        #endregion

        #region Sprite

        public Sprite GetSprite(string spriteName)
        {
            string assetPath = spritePathProxy.GetAssetPath(spriteName, out bool isMultiple);
            if (string.IsNullOrEmpty(assetPath))
            {
                MDebug.Error($"没有找到精灵对应的资源路径 : {spriteName}");
                return null;
            }
            if (isMultiple)
            {
                var entry = CreateAssetEntry<SubAssetEntry>(assetPath);
                if (entry == null)
                {
                    return null;
                }
                Sprite sprite = entry.GetSubAssetObject<Sprite>(spriteName);
                ActiveAsset(sprite, assetPath);
                return sprite;
            }
            else
            {
                var entry = CreateAssetEntry<SpriteAtlasAssetEntry>(assetPath);
                if (entry == null)
                {
                    return null;
                }
                Sprite sprite = entry.GetSprite(spriteName);
                ActiveAsset(sprite, assetPath);
                return sprite;
            }
        }

        public async UniTask<Sprite> GetSpriteAsync(string spriteName)
        {
            string assetPath = spritePathProxy.GetAssetPath(spriteName, out bool isMultiple);
            if (string.IsNullOrEmpty(assetPath))
            {
                MDebug.Error($"没有找到精灵对应的资源路径 : {spriteName}");
                return null;
            }
            if (isMultiple)
            {
                var entry = await CreateAssetEntryAsync<SubAssetEntry>(assetPath);
                if (entry == null)
                {
                    return null;
                }
                return entry.GetSubAssetObject<Sprite>(spriteName);
            }
            else
            {
                var entry = await CreateAssetEntryAsync<SpriteAtlasAssetEntry>(assetPath);
                if (entry == null)
                {
                    return null;
                }
                return entry.GetSprite(spriteName);
            }
        }

        #endregion

        public void ReleaseAsset(UnityEngine.Object asset)
        {
            if (asset == null)
            {
                return;
            }

            int instanceID = asset.GetInstanceID();
            if (!activeEntries.TryGetValue(instanceID, out var assetPath))
            {
                return;
            }

            if (!assetEntries.TryGetValue(assetPath, out var entry))
            {
                return;
            }

            // 只做逻辑层减引用，归零的 Entry 暂时保留作缓存，handle 不立即释放，
            // 留待 UnloadUnusedAssetsAsync 在安全时机统一回收，避免“放了又拿”造成的加载抖动。
            if (entry.ReleaseAsset(asset))
            {
                MDebug.Log($"资源释放 : {instanceID} => {assetPath}");
                activeEntries.Remove(instanceID);
            }
        }

        public async UniTask UnloadUnusedAssetsAsync()
        {
            // 仍有异步加载在途时跳过
            if (loadingEntries.Count > 0)
            {
                return;
            }

            // 释放逻辑层已无引用的Entry
            unloadBuffer.Clear();
            foreach (var kv in assetEntries)
            {
                if (kv.Value.RefCount <= 0)
                {
                    unloadBuffer.Add(kv.Key);
                }
            }

            MDebug.Log($"开始卸载资源，总数 = {unloadBuffer.Count}");
            for (int i = 0; i < unloadBuffer.Count; i++)
            {
                string assetPath = unloadBuffer[i];
                if (assetEntries.TryGetValue(assetPath, out var entry))
                {
                    assetEntries.Remove(assetPath);
                    entry.Release();
                    MDebug.Log($"卸载资源 : {assetPath}");
                }
            }
            unloadBuffer.Clear();
            await provider.UnloadUnusedAssetsAsync();

            MDebug.Log($"卸载资源完成！");
        }

        private T CreateAssetEntry<T>(string assetPath) where T : AssetEntry, new()
        {
            if (assetEntries.TryGetValue(assetPath, out AssetEntry existing))
            {
                return Cast<T>(existing, assetPath);
            }

            // 已有异步加载在途：接管其句柄并强制同步完成，避免重复创建 handle 造成泄漏。
            // 句柄的完成判定与失败释放统一收敛在 AssetEntry 内部单点处理，这里只需要根据成败决定是否登记。
            // 失败时不登记，loadingEntries 的清理交由异步侧的 finally 完成，避免重复清理。
            if (loadingEntries.TryGetValue(assetPath, out AssetEntry loading))
            {
                if (loading.WaitForAsyncComplete())
                {
                    assetEntries[assetPath] = loading;
                    return Cast<T>(loading, assetPath);
                }
                return null;
            }

            T entry = new T();
            entry.SetProvider(provider);
            if (entry.LoadAsset(assetPath))
            {
                assetEntries[assetPath] = entry;
                return entry;
            }
            return null;
        }

        private async UniTask<T> CreateAssetEntryAsync<T>(string assetPath) where T : AssetEntry, new()
        {
            if (assetEntries.TryGetValue(assetPath, out AssetEntry existing))
            {
                return Cast<T>(existing, assetPath);
            }

            if (loadingEntries.TryGetValue(assetPath, out AssetEntry loading))
            {
                bool ok = await loading.Task;
                if (!ok)
                {
                    return null;
                }
                return Cast<T>(loading, assetPath);
            }

            T entry = new T();
            entry.SetProvider(provider);
            loadingEntries[assetPath] = entry;

            try
            {
                bool success = await entry.LoadAssetAsync(assetPath);
                if (!success)
                {
                    return null;
                }

                if (assetEntries.TryGetValue(assetPath, out existing))
                {
                    return Cast<T>(existing, assetPath);
                }

                assetEntries[assetPath] = entry;
                return entry;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[GameAssetsManager] 异步加载资源异常: {assetPath}\n{e}");
                return null;
            }
            finally
            {
                loadingEntries.Remove(assetPath);
            }
        }

        // 同一 assetPath 被以不同 Entry 类型请求时，显式报错。
        private static T Cast<T>(AssetEntry entry, string assetPath) where T : AssetEntry
        {
            if (entry is T typed)
            {
                return typed;
            }
            Debug.LogError($"[GameAssetsManager] 资源类型不匹配: {assetPath} 已作为 {entry.GetType().Name} 加载, 无法作为 {typeof(T).Name} 获取");
            return null;
        }

        private void ActiveAsset(UnityEngine.Object asset, string assetPath)
        {
            if (asset == null)
            {
                return;
            }
            activeEntries[asset.GetInstanceID()] = assetPath;
        }
    }
}
