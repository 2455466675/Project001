using System.Linq;
using YooAsset;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFramework.Core 
{
    public class ResourceManager : IGameModule
    {
        private ResourcePackage m_Package;

        public GameModulePriority Priority => GameModulePriority.ResourceManager;

        //private SpriteManager spriteManager;

        public async UniTask Init()
        {
            string packageName = "DefaultPackage";

            YooAssets.Initialize();

            m_Package = YooAssets.TryGetPackage(packageName);
            m_Package ??= YooAssets.CreatePackage(packageName);

            YooAssets.SetDefaultPackage(m_Package);

            RuntimePlatform platform = Application.platform;

            if (platform == RuntimePlatform.WindowsEditor)
            {
                var buildResult = EditorSimulateModeHelper.SimulateBuild(packageName);
                var packageRoot = buildResult.PackageRootDirectory;
                var editorFileSystemParams = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
                var initParameters = new EditorSimulateModeParameters();
                initParameters.EditorFileSystemParameters = editorFileSystemParams;

                await m_Package.InitializeAsync(initParameters);
            }
            else if (platform == RuntimePlatform.WindowsPlayer)
            {
                var buildinFileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
                var initParameters = new OfflinePlayModeParameters();
                initParameters.BuildinFileSystemParameters = buildinFileSystemParams;

                await m_Package.InitializeAsync(initParameters);
            }

            //spriteManager = new SpriteManager(initCfg.SpriteMap);
        }

        /// <summary>
        /// 从Resources下加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadFormRes<T>(string path) where T : UnityEngine.Object
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
            return m_Package.LoadAssetSync<T>(path).AssetObject as T;
        }

        /// <summary>
        /// 异步加载资源（Assets下的路径）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            AssetHandle handle = m_Package.LoadAssetAsync<T>(path);
            await handle.Task;
            return handle.GetAssetObject<T>();
        }

        public T[] LoadAllAssets<T>(string path) where T : UnityEngine.Object
        {
            AllAssetsHandle handle = m_Package.LoadAllAssetsSync<T>(path);
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
            //return GoHelper.Instantiate(obj, parent);
            return null;
        }

        public async UniTask<GameObject> LoadAndInstantiateAsync(string path, Transform parent)
        {
            GameObject obj = await LoadAssetAsync<GameObject>(path);
            //return GoHelper.Instantiate(obj, parent);
            return null;
        }

        public Sprite GetSprite(string spriteName)
        {
            //return spriteManager.GetSprite(spriteName);
            return null;
        }
    }
}