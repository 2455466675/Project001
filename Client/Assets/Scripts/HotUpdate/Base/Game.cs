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
    public struct GameStartUpEventArgs : IGameEventArgs
    {

    }

    public static class Game
    {
        private class GameModule 
        {
            public Type type;
            public int priority;
            public IGameModule obj;
        }

        private static bool isStarted;
        private static Dictionary<Type, GameModule> m_GameModules;
        private static List<IUpdate> m_UpdateableModules;

        private static List<Assembly> m_Assemblies;
        private static Dictionary<Type, List<Type>> m_AttributeTypes;

        public static EventManager Event { get; private set; }

        static Game()
        {
            m_GameModules = new Dictionary<Type, GameModule>();
            m_UpdateableModules = new List<IUpdate>();
            m_Assemblies = new List<Assembly>();
            m_AttributeTypes = new Dictionary<Type, List<Type>>();
            Event = new EventManager();
        }

        public static async void Start()
        {
            MDebug.Log("Game Start!");
            isStarted = false;

            await LoadMetadataForAOTAssembly();
            await LoadHotUpdateAssemblies();

            InitGameAttributeTypes();

            await InitGameModules();
            await LoadGameRoot();

            Event.Init();
            isStarted = true;

            Event.Publish(new GameStartUpEventArgs());
        }

        public static void Update() 
        {
            if (!isStarted) 
            {
                return;
            }

            for (int i = 0; i < m_UpdateableModules.Count; i++)
            {
                m_UpdateableModules[i].Update();
            }
        }

        public static T GetModule<T>() where T : class, IGameModule
        {
            Type type = typeof(T);
            if (m_GameModules.TryGetValue(type, out GameModule module))
            {
                return module.obj as T;
            }
            else
            {
                return default;
            }
        }

        public static Type[] GetTypes<T>() where T : GameAttribute
        {
            Type type = typeof(T);
            if (m_AttributeTypes.ContainsKey(type))
            {
                return m_AttributeTypes[type].ToArray();
            }
            else
            {
                return new Type[0];
            }
        }

        private static async UniTask LoadMetadataForAOTAssembly()
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
                var handle = YooAssets.LoadAssetAsync<TextAsset>(path);
                await handle;

                byte[] dllBytes = handle.GetAssetObject<TextAsset>().bytes;
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);

                Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
            }

#else
            await UniTask.Yield();
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
            m_Assemblies = assemblies;
        }

        private static void InitGameAttributeTypes() 
        {
            m_AttributeTypes.Clear();

            foreach (var assembly in m_Assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    var attributes = type.GetCustomAttributes(typeof(GameAttribute), false);
                    foreach (var attribute in attributes)
                    {
                        Type attributeType = attribute.GetType();
                        if (!m_AttributeTypes.TryGetValue(attributeType, out List<Type> list))
                        {
                            list = new List<Type>();
                            m_AttributeTypes.Add(attributeType, list);
                        }

                        list.Add(type);
                    }
                }
            }
        }

        private static async UniTask InitGameModules()
        {
            m_GameModules.Clear();
            m_UpdateableModules.Clear();

            List<GameModule> modules = new List<GameModule>();

            Type[] types = GetTypes<GameModuleAttribute>();
            foreach (Type type in types)
            {
                if (m_GameModules.ContainsKey(type))
                {
                    continue;
                }

                var attribute = type.GetCustomAttribute(typeof(GameModuleAttribute), false) as GameModuleAttribute;
                var obj = Activator.CreateInstance(type);

                GameModule module = new GameModule
                {
                    type = type,
                    obj = obj as IGameModule,
                    priority = (int)attribute.Priority
                };

                modules.Add(module);
                m_GameModules.Add(type, module);
            }

            modules.Sort((a, b) => a.priority - b.priority);

            foreach (var module in modules)
            {
                IGameModule obj = module.obj;
                if (obj is ISyncInit s)
                {
                    s.Init();
                }

                if (obj is IAsyncInit a)
                {
                    await a.Init();
                }

                if (obj is IUpdate u)
                {
                    m_UpdateableModules.Add(u);
                }

                Debug.Log("GameModule Init : " + module.type.Name);
            }
        }

        private static async UniTask LoadGameRoot() 
        {
            var handle = YooAssets.LoadAssetAsync<GameObject>("Assets/Bundles/Common/GameRoot");
            await handle;
            await handle.InstantiateAsync();
        }
    }
}