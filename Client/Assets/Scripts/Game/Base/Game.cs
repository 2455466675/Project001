using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

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
    }
}