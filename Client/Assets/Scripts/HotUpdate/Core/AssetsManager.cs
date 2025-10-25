using System.Linq;
using YooAsset;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace GameFramework.Core 
{
    [GameModule]
    public class AssetsManager : IGameModule_SyncInit
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
            var handle = YooAssets.LoadAssetSync<T>(path);
            T result = handle.GetAssetObject<T>();
            handle.Release();
            return result;
        }

        /// <summary>
        /// 异步加载资源（Assets下的路径）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            var handle = YooAssets.LoadAssetAsync<T>(path);
            await handle;
            T result = handle.GetAssetObject<T>();
            handle.Release();
            return result;
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

        /*
         * 会把字体资源也卸载了。。。。
         */
        public async UniTask UnloadUnusedAssetsAsync() 
        {
            var package = YooAssets.GetPackage("DefaultPackage");
            await package.UnloadUnusedAssetsAsync();
        }

        public Sprite GetSprite(string spriteName)
        {
            //return spriteManager.GetSprite(spriteName);
            return null;
        }
    }
}