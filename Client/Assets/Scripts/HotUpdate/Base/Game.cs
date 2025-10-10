using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using HybridCLR;
using YooAsset;
using System.Reflection;

namespace GameFramework
{
    public static class Game
    {
        private static Dictionary<Type, IGameModule> m_GameModules;
        private static List<IUpdate> m_UpdateableModules;

        static Game()
        {
            m_GameModules = new Dictionary<Type, IGameModule>();
            m_UpdateableModules = new List<IUpdate>();
        }

        public static void Start()
        {
            MDebug.Log("Game Start!");
            LoadMetadataForAOTAssembly();

            LoadHotUpdateAssemblies().Forget();
        }

        public static void AddModule<T>() where T : class, IGameModule, new()
        {
            Type type = typeof(T);
            if (m_GameModules.ContainsKey(type))
            {
                return;
            }

            T module = new();
            m_GameModules.Add(type, module);

            if (module is IUpdate u)
            {
                m_UpdateableModules.Add(u);
            }
        }

        public static T GetModule<T>() where T : class, IGameModule
        {
            Type type = typeof(T);
            if (m_GameModules.TryGetValue(type, out IGameModule module))
            {
                return module as T;
            }
            else
            {
                return default;
            }
        }

        public static async UniTask InitModules()
        {
            List<IGameModule> modules = m_GameModules.Values.ToList();
            modules.Sort((a, b) => a.Priority.CompareTo(b.Priority));

            foreach (var module in modules)
            {
                await module.Init();
            }
        }

        private static void LoadMetadataForAOTAssembly()
        {
#if !UNITY_EDITOR

            List<string> aotDllList = new List<string>
            {
                "mscorlib",
                "System",
                "System.Core",
                "UniTask",
                "Unity.InputSystem",
                "UnityEngine.CoreModule",
                "YooAsset",
            };

            HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in aotDllList)
            {
                string path = string.Format("Assets/Bundles/Dlls/{0}.dll", aotDllName);
                byte[] dllBytes = YooAssets.LoadAssetSync<TextAsset>(path).GetAssetObject<TextAsset>().bytes;
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
                Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
            }
#endif
        }

        private static async UniTask LoadHotUpdateAssemblies()
        {
            List<string> hotUpdateDllList = new List<string>
            {
                "GameConfig",
                "HotUpdate_Utility",
                "HotUpdate_Core",
                "HotUpdate_Feature",
                "HotUpdate_Gameplay",
                "HotUpdate_UI",
            };

            List<Assembly> assemblies = new List<Assembly>();

#if UNITY_EDITOR
            foreach (var name in hotUpdateDllList)
            {
                Assembly assembly = Assembly.Load(name);
                assemblies.Add(assembly);
                Debug.Log("Load Assembly : " + assembly.GetName().Name);
            }
            await UniTask.Yield();
#else
            foreach (var name in hotUpdateDllList)
            {
                string path = string.Format("Assets/Bundles/Dlls/{0}.dll", name);
                var handle = YooAssets.LoadAssetAsync(path);
                await handle;

                TextAsset textAsset = handle.GetAssetObject<TextAsset>();
                Assembly assembly = Assembly.Load(textAsset.bytes);
                assemblies.Add(assembly);
                Debug.Log("Load Assembly : " + assembly.GetName().Name);
            }
#endif
        }
    }
}