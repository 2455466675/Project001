using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D;
using Cysharp.Threading.Tasks;

namespace GameFramework.Core
{
    internal abstract class AssetEntry
    {
        // 加载结果通知源：同步、异步、以及"同步接管在途异步"三条路径最终都从这里拿到成败结果，
        // 失败时等待者得到 false 而非一个句柄已被释放的无效 Entry。
        private readonly UniTaskCompletionSource<bool> source = new UniTaskCompletionSource<bool>();

        // isFinalized 保证完成判定与句柄释放只执行一次：避免同步侧强制完成与异步侧 await 唤醒后各自再释放一次句柄，
        // 这正是原实现中双重 Release / 空引用崩溃的根因。
        private bool isFinalized;
        private bool isLoaded;
        private string loadPath;

        // 资源后端由外部注入，Entry 不感知具体框架（YooAsset 等）。
        protected IAssetProvider Provider { get; private set; }

        public UniTask<bool> Task => source.Task;
        public bool IsDone => isLoaded;
        protected string LoadPath => loadPath;
        public abstract int RefCount { get; }

        internal void SetProvider(IAssetProvider provider)
        {
            Provider = provider;
        }

        public bool LoadAsset(string assetPath)
        {
            if (isLoaded)
            {
                return true;
            }
            loadPath = assetPath;
            CreateHandleSync(assetPath);
            return CompleteLoad();
        }

        public async UniTask<bool> LoadAssetAsync(string assetPath)
        {
            if (isLoaded)
            {
                return true;
            }
            loadPath = assetPath;
            CreateHandleAsync(assetPath);
            await WaitHandleAsync();
            return CompleteLoad();
        }

        // 供同步流程接管一个仍在途的异步句柄：强制其同步完成后走统一的完成判定。
        // 若结果失败，CompleteLoad 会单点释放句柄，异步侧醒来后因 isFinalized 已置位而直接返回，不再触碰句柄。
        public bool WaitForAsyncComplete()
        {
            if (isLoaded)
            {
                return true;
            }
            if (!HasHandle())
            {
                return false;
            }
            WaitHandleSync();
            return CompleteLoad();
        }

        private bool CompleteLoad()
        {
            if (isFinalized)
            {
                return isLoaded;
            }
            isFinalized = true;

            if (IsHandleSucceed)
            {
                isLoaded = true;
                source.TrySetResult(true);
                return true;
            }

            MDebug.Error($"[GameAssetsManager] 加载资源失败: {loadPath}, {LastError}");
            ReleaseHandle();
            source.TrySetResult(false);
            return false;
        }

        // 句柄释放后重置加载标记，防止已释放的 Entry 被误判为可用。
        protected void MarkReleased()
        {
            isLoaded = false;
            isFinalized = false;
        }

        public abstract bool ReleaseAsset(UnityEngine.Object asset);
        public abstract void Release();

        protected abstract bool HasHandle();
        protected abstract void CreateHandleSync(string assetPath);
        protected abstract void CreateHandleAsync(string assetPath);
        protected abstract UniTask WaitHandleAsync();
        protected abstract void WaitHandleSync();
        protected abstract bool IsHandleSucceed { get; }
        protected abstract string LastError { get; }
        protected abstract void ReleaseHandle();
    }

    // 单资源句柄（AssetObject）：Raw / Prefab / SpriteAtlas 共用。
    internal abstract class SingleAssetEntry : AssetEntry
    {
        protected IAssetHandle handle;

        protected override bool HasHandle()
        {
            return handle != null;
        }

        protected override void CreateHandleSync(string assetPath)
        {
            handle = Provider.LoadAssetSync(assetPath);
        }

        protected override void CreateHandleAsync(string assetPath)
        {
            handle = Provider.LoadAssetAsync(assetPath);
        }

        protected override async UniTask WaitHandleAsync()
        {
            // 用局部引用持有句柄：即使同步侧在接管时把字段置空，这里 await 的仍是同一个已完成对象，避免空引用。
            var h = handle;
            if (h != null)
            {
                await h.WaitAsync();
            }
        }

        protected override void WaitHandleSync()
        {
            handle.WaitForAsyncComplete();
        }

        protected override bool IsHandleSucceed => handle != null && handle.IsSucceed;
        protected override string LastError => handle == null ? string.Empty : handle.LastError;

        protected override void ReleaseHandle()
        {
            if (handle != null)
            {
                handle.Release();
                handle = null;
            }
            MarkReleased();
        }
    }

    // 子资源句柄（SubAssets）。
    internal abstract class SubAssetsEntry : AssetEntry
    {
        protected ISubAssetsHandle handle;

        protected override bool HasHandle()
        {
            return handle != null;
        }

        protected override void CreateHandleSync(string assetPath)
        {
            handle = Provider.LoadSubAssetsSync(assetPath);
        }

        protected override void CreateHandleAsync(string assetPath)
        {
            handle = Provider.LoadSubAssetsAsync(assetPath);
        }

        protected override async UniTask WaitHandleAsync()
        {
            var h = handle;
            if (h != null)
            {
                await h.WaitAsync();
            }
        }

        protected override void WaitHandleSync()
        {
            handle.WaitForAsyncComplete();
        }

        protected override bool IsHandleSucceed => handle != null && handle.IsSucceed;
        protected override string LastError => handle == null ? string.Empty : handle.LastError;

        protected override void ReleaseHandle()
        {
            if (handle != null)
            {
                handle.Release();
                handle = null;
            }
            MarkReleased();
        }
    }

    internal class RawAssetEntry : SingleAssetEntry
    {
        public override int RefCount => refCount;
        private int refCount;

        public T GetAsset<T>() where T : UnityEngine.Object
        {
            if (!IsDone)
            {
                return default;
            }

            T asset = handle.AssetObject as T;
            if (asset != null)
            {
                refCount++;
            }
            else
            {
                MDebug.Error($"资源类型不匹配:{handle.AssetType?.Name} to {typeof(T).Name}");
            }

            return asset;
        }

        public override bool ReleaseAsset(Object asset)
        {
            if (!IsDone)
            {
                return false;
            }
            if (asset == null)
            {
                return false;
            }
            int instanceID = asset.GetInstanceID();
            if (instanceID == handle.AssetObject.GetInstanceID())
            {
                refCount--;
            }
            return refCount <= 0;
        }

        public override void Release()
        {
            if (!IsDone)
            {
                return;
            }

            if (RefCount <= 0)
            {
                refCount = 0;
                ReleaseHandle();
            }
            else
            {
                MDebug.Error($"资源还被引用着！无法释放！RefCount = {RefCount}; path = {LoadPath}");
            }
        }
    }

    internal class PrefabAssetEntry : SingleAssetEntry
    {
        public override int RefCount => instances.Count;
        private readonly HashSet<int> instances;

        public PrefabAssetEntry()
        {
            instances = new HashSet<int>();
        }

        // 实例化与实例销毁均由本类自行管理（见 ReleaseAsset 的 Destroy），因此直接用 Object.Instantiate，
        // 不依赖具体后端的实例化能力，便于更换资源框架。
        private GameObject Prefab => handle == null ? null : handle.AssetObject as GameObject;

        public GameObject Instantiate()
        {
            if (!IsDone)
            {
                return null;
            }
            GameObject gameObject = Object.Instantiate(Prefab);
            AddInstance(gameObject);
            return gameObject;
        }

        public GameObject Instantiate(Transform parent)
        {
            if (!IsDone)
            {
                return null;
            }
            GameObject gameObject = Object.Instantiate(Prefab, parent);
            AddInstance(gameObject);
            return gameObject;
        }

        public GameObject Instantiate(Transform parent, bool worldPositionStays)
        {
            if (!IsDone)
            {
                return null;
            }
            GameObject gameObject = Object.Instantiate(Prefab, parent, worldPositionStays);
            AddInstance(gameObject);
            return gameObject;
        }

        public GameObject Instantiate(Vector3 position, Quaternion rotation)
        {
            if (!IsDone)
            {
                return null;
            }
            GameObject gameObject = Object.Instantiate(Prefab, position, rotation);
            AddInstance(gameObject);
            return gameObject;
        }

        public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!IsDone)
            {
                return null;
            }
            GameObject gameObject = Object.Instantiate(Prefab, position, rotation, parent);
            AddInstance(gameObject);
            return gameObject;
        }

        private void AddInstance(UnityEngine.Object asset)
        {
            if (asset == null)
            {
                return;
            }
            instances.Add(asset.GetInstanceID());
        }

        public override bool ReleaseAsset(Object asset)
        {
            if (asset == null)
            {
                return false;
            }

            int instanceID = asset.GetInstanceID();
            if (instances.Remove(instanceID))
            {
                UnityEngine.Object.Destroy(asset);
                return true;
            }
            return RefCount <= 0;
        }

        public override void Release()
        {
            if (!IsDone)
            {
                return;
            }

            if (RefCount <= 0)
            {
                instances.Clear();
                ReleaseHandle();
            }
            else
            {
                MDebug.Error($"资源还被引用着！无法释放！RefCount = {RefCount}; path = {LoadPath}");
            }
        }
    }

    internal class SpriteAtlasAssetEntry : SingleAssetEntry
    {
        public override int RefCount => instances.Count;

        private readonly HashSet<int> instances;
        private SpriteAtlas atlas;

        public SpriteAtlasAssetEntry()
        {
            instances = new HashSet<int>();
        }

        public Sprite GetSprite(string spriteName)
        {
            if (!IsDone)
            {
                return default;
            }

            if (atlas == null)
            {
                atlas = handle.AssetObject as SpriteAtlas;
                if (atlas == null)
                {
                    return default;
                }
            }
            Sprite sprite = atlas.GetSprite(spriteName); //这里获取到的是克隆体，ReleaseAsset时需要销毁
            AddInstance(sprite);
            return sprite;
        }

        private void AddInstance(UnityEngine.Object asset)
        {
            if (asset == null)
            {
                return;
            }
            instances.Add(asset.GetInstanceID());
        }

        public override bool ReleaseAsset(Object asset)
        {
            if (asset == null)
            {
                return false;
            }

            int instanceID = asset.GetInstanceID();
            if (instances.Remove(instanceID))
            {
                UnityEngine.Object.Destroy(asset);
                return true;
            }
            return RefCount <= 0;
        }

        public override void Release()
        {
            if (!IsDone)
            {
                return;
            }

            if (RefCount <= 0)
            {
                atlas = null;
                instances.Clear();
                ReleaseHandle();
            }
            else
            {
                MDebug.Error($"资源还被引用着！无法释放！RefCount = {RefCount}; path = {LoadPath}");
            }
        }
    }

    internal class SubAssetEntry : SubAssetsEntry
    {
        public override int RefCount
        {
            get
            {
                int count = 0;
                foreach (var item in subAssetRef)
                {
                    count += item.Value;
                }
                return count;
            }
        }

        private Dictionary<int, int> subAssetRef;

        public SubAssetEntry()
        {
            subAssetRef = new Dictionary<int, int>();
        }

        public T GetSubAssetObject<T>(string name) where T : UnityEngine.Object
        {
            if (!IsDone)
            {
                return default;
            }

            var asset = handle.GetSubAssetObject<T>(name); //这里获得的原始资源的引用，ReleaseAsset时不要销毁
            if (asset != null)
            {
                int instanceId = asset.GetInstanceID();
                if (subAssetRef.ContainsKey(instanceId))
                {
                    subAssetRef[instanceId] = subAssetRef[instanceId] + 1;
                }
                else
                {
                    subAssetRef[instanceId] = 1;
                }
            }
            return asset;
        }

        public override bool ReleaseAsset(Object asset)
        {
            if (asset == null)
            {
                return false;
            }

            int instanceId = asset.GetInstanceID();
            if (subAssetRef.ContainsKey(instanceId))
            {
                subAssetRef[instanceId] = Mathf.Max(0, subAssetRef[instanceId] - 1);
            }
            return RefCount <= 0;
        }

        public override void Release()
        {
            if (!IsDone)
            {
                return;
            }

            if (RefCount <= 0)
            {
                subAssetRef.Clear();
                ReleaseHandle();
            }
            else
            {
                MDebug.Error($"资源还被引用着！无法释放！RefCount = {RefCount}; path = {LoadPath}");
            }
        }
    }
}
