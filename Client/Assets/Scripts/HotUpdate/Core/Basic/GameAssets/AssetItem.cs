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
            refCount++;

            return asset;
        }

        public void ReleaseAsset(UnityEngine.Object asset)
        {
            if (asset == null)
            {
                return;
            }

            if (!IsDone)
            {
                return;
            }

            int instanceID = asset.GetInstanceID();
            if (instances.Contains(instanceID))
            {
                instances.Remove(instanceID);
                UnityEngine.Object.Destroy(asset);
            }
            else
            {
                if (instanceID == handle.AssetObject.GetInstanceID())
                {
                    refCount--;
                }                
            }
        }

        public void Release()
        {
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