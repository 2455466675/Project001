using System.Linq;
using YooAsset;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFramework.Core 
{
    [GameModule(GameModulePriority.AssetsManager)]
    public class AssetsManager : ISyncInit
    {
        //private SpriteManager spriteManager;

        public void Init()
        {
            //spriteManager = new SpriteManager(initCfg.SpriteMap);
        }

        /// <summary>
        /// 从Resources下加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadFormResources<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 加载资源（Assets下的路径）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return YooAssets.LoadAssetSync<T>(path).AssetObject as T;
        }

        /// <summary>
        /// 异步加载资源（Assets下的路径）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            AssetHandle handle = YooAssets.LoadAssetAsync<T>(path);
            await handle.Task;
            return handle.GetAssetObject<T>();
        }

        public T[] LoadAllAssets<T>(string path) where T : UnityEngine.Object
        {
            AllAssetsHandle handle = YooAssets.LoadAllAssetsSync<T>(path);
            return handle.AllAssetObjects.Cast<T>().ToArray();
        }

        /// <summary>
        /// 加载并实例化一个游戏物体（Assets下的路径）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
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

        public Sprite GetSprite(string spriteName)
        {
            //return spriteManager.GetSprite(spriteName);
            return null;
        }
    }
}