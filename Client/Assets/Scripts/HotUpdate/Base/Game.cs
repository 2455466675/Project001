using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using HybridCLR;
using YooAsset;
using System.Reflection;
using LITJson;

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
        private static List<IFixedUpdate> m_FixedUpdateableModules;
        private static List<ILateUpdate> m_LateUpdateableModules;

        private static List<Assembly> m_Assemblies;
        private static Dictionary<Type, List<Type>> m_AttributeTypes;

        public static IGameEvent Event { get; private set; }
        private static IGameplay Gameplay { get; }

        public static float DeltaTime
        {
            get
            {
                return Time.deltaTime;
            }
        }

        static Game()
        {
            m_GameModules = new Dictionary<Type, GameModule>();

            m_UpdateableModules = new List<IUpdate>();
            m_FixedUpdateableModules = new List<IFixedUpdate>();
            m_LateUpdateableModules = new List<ILateUpdate>();

            m_Assemblies = new List<Assembly>();
            m_AttributeTypes = new Dictionary<Type, List<Type>>();
            Event = new GameEvent();
            Gameplay = new Gameplay();
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
            await Gameplay.Init();
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

        public static void FixedUpdate()
        {
            if (!isStarted)
            {
                return;
            }
            for (int i = 0; i < m_FixedUpdateableModules.Count; i++)
            {
                m_FixedUpdateableModules[i].FixedUpdate();
            }
        }

        public static void LateUpdate()
        {
            if (!isStarted)
            {
                return;
            }
            for (int i = 0; i < m_LateUpdateableModules.Count; i++)
            {
                m_LateUpdateableModules[i].LateUpdate();
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

        #region Gameplay

        public static T GetSystem<T>() where T : class, IGameplaySystem
        {
            return Gameplay.GetSystem<T>();
        }

        public static void Exit() 
        {
            Gameplay.Exit();
        }
        
        public static void SaveGame(ISaveWriter writer)
        {
            Gameplay.SaveGame(writer);
        }

        public static void LoadGame(ISaveReader reader)
        {
            Gameplay.LoadGame(reader);
        }

        #endregion

        /// <summary>
        /// 获取游戏模块的优先级
        /// </summary>
        /// <param name="name">特性名</param>
        /// <returns></returns>
        internal static async UniTask<List<ClassPriorityData>> GetGamePriorityDatas(string name)
        {
            var path = string.Format("Assets/Bundles/Common/{0}.json", name);
            var handle = YooAssets.LoadAssetAsync<TextAsset>(path);
            await handle;
            if (handle.AssetObject == null)
            {
                return new List<ClassPriorityData>();
            }
            else
            {
                var json = handle.GetAssetObject<TextAsset>().text;
                ClassPriorityListWrapper wrapper = JsonMapper.ToObject<ClassPriorityListWrapper>(json);
                List<ClassPriorityData> items = wrapper != null && wrapper.items != null ? wrapper.items : new List<ClassPriorityData>();
                return items;
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
            m_FixedUpdateableModules.Clear();
            m_LateUpdateableModules.Clear();

            List<ClassPriorityData> items = await GetGamePriorityDatas("GameModuleAttribute");
            int GetPriority(string fullName)
            {
                foreach (var data in items)
                {
                    if (data.type == fullName)
                    {
                        return data.priority;
                    }
                }
                return 0;
            }

            List<GameModule> modules = new List<GameModule>();

            Type[] types = GetTypes<GameModuleAttribute>();
            foreach (Type type in types)
            {
                if (m_GameModules.ContainsKey(type))
                {
                    continue;
                }

                var obj = Activator.CreateInstance(type);

                GameModule module = new GameModule
                {
                    type = type,
                    obj = obj as IGameModule,
                    priority = GetPriority(type.FullName),
                };

                modules.Add(module);
                m_GameModules.Add(type, module);
            }

            modules.Sort((a, b) => a.priority - b.priority);

            foreach (var module in modules)
            {
                IGameModule obj = module.obj;
                if (obj is IGameModule_SyncInit s)
                {
                    s.Init();
                }

                if (obj is IGameModule_AsyncInit a)
                {
                    await a.Init();
                }

                if (obj is IUpdate u)
                {
                    AddIUpdate(u);
                }

                if (obj is IFixedUpdate fu)
                {
                    AddIFixedUpdate(fu);
                }

                if (obj is ILateUpdate lu)
                {
                    AddILateUpdate(lu);
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

        internal static void AddIUpdate(IUpdate u)
        {
            m_UpdateableModules.Add(u);
        }

        internal static void AddIFixedUpdate(IFixedUpdate fu)
        {
            m_FixedUpdateableModules.Add(fu);
        }

        internal static void AddILateUpdate(ILateUpdate lu)
        {
            m_LateUpdateableModules.Add(lu);
        }
    }
}