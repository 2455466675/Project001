using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace GameFramework.Core
{
    internal class AssetItem
    {
        public bool IsDone => handle != null && handle.IsValid && handle.IsDone;
        public int RefCount => instances.Count + refCount;
        private int refCount;

        private HashSet<int> instances;
        private AssetHandle handle;

        internal AssetItem(AssetHandle handle)
        {
            refCount = 0;
            instances = new HashSet<int>();
            this.handle = handle;
        }

        public T Instantiate<T>() where T : UnityEngine.Object
        {
            if (!IsDone)
            {
                return default;
            }

            var asset = UnityEngine.Object.Instantiate(handle.AssetObject);
            AddInstance(asset);
            return asset as T;
        }

        public GameObject Instantiate()
        {
            if (!IsDone)
            {
                return null;
            }
            GameObject gameObject = handle.InstantiateSync();
            AddInstance(gameObject);
            return gameObject;
        }

        public GameObject Instantiate(Transform parent)
        {
            if (!IsDone)
            {
                return null;
            }

            GameObject gameObject = handle.InstantiateSync(parent);
            AddInstance(gameObject);
            return gameObject;
        }

        public GameObject Instantiate(Transform parent, bool worldPositionStays)
        {
            if (!IsDone)
            {
                return null;
            }
            GameObject gameObject = handle.InstantiateSync(parent, worldPositionStays);
            AddInstance(gameObject);
            return gameObject;
        }

        public GameObject Instantiate(Vector3 position, Quaternion rotation)
        {
            if (!IsDone)
            {
                return null;
            }
            GameObject gameObject = handle.InstantiateSync(position, rotation);
            AddInstance(gameObject);
            return gameObject;
        }

        public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!IsDone)
            {
                return null;
            }
            GameObject gameObject = handle.InstantiateSync(position, rotation, parent);
            AddInstance(gameObject);
            return gameObject;
        }

        public T GetAsset<T>() where T : UnityEngine.Object
        {
            if (!IsDone)
            {
                return default;
            }

            T asset = handle.AssetObject as T;
            if (asset == null)
            {
                MDebug.Error($"资源类型不匹配:{handle.GetAssetInfo().AssetType.Name} to {typeof(T).Name}");
            }
            else
            {
                refCount++;
            }
            
            return asset;
        }

        public bool ReleaseAsset(UnityEngine.Object asset)
        {
            if (asset == null)
            {
                return false;
            }

            int instanceID = asset.GetInstanceID();

            // 实例对象是独立于 handle 的真实对象，其销毁不应受 handle 状态影响，必须始终执行，否则会残留 GameObject
            if (instances.Remove(instanceID))
            {
                UnityEngine.Object.Destroy(asset);
                return true;
            }

            if (handle == null || !handle.IsValid || !handle.IsDone)
            {
                return false;
            }

            if (instanceID == handle.AssetObject.GetInstanceID())
            {
                refCount--;
            }

            return refCount <= 0;
        }

        public void Release()
        {
            if (handle == null)
            {
                return;
            }

            if (RefCount <= 0)
            {
                handle.Release();
                handle = null;
            }
        }

        private void AddInstance(UnityEngine.Object asset)
        {
            if (asset == null)
            {
                return;
            }
            instances.Add(asset.GetInstanceID());
        }
    }
}