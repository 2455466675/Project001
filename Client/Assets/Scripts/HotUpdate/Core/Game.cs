using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using HybridCLR;
using YooAsset;

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

        public static void LoadMetadataForAOTAssembly()
        {
#if !UNITY_EDITOR

            List<string> aotDllList = new List<string>
            {
                "mscorlib.dll",
                "System.dll",
                "System.Core.dll",
                "UniTask.dll",
                "Unity.InputSystem.dll",
                "UnityEngine.CoreModule.dll",
                "YooAsset.dll",
            };

            HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in aotDllList)
            {
                string path = string.Format("Assets/Bundles/Dlls/{0}", aotDllName);
                byte[] dllBytes = YooAssets.LoadAssetSync<TextAsset>(path).GetAssetObject<TextAsset>().bytes;
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
                Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
            }
#endif
        }

    }
}