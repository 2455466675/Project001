using System;
using System.Reflection;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameFramework.Core 
{
    public class AssemblyManager : IGameModule
    {
        private static List<Assembly> m_Assemblies = new List<Assembly>();
        public static void AddAssembly(Assembly assembly) 
        {
            if (m_Assemblies.Contains(assembly)) 
            {
                return;
            }
            m_Assemblies.Add(assembly);
        }

        public GameModulePriority Priority => GameModulePriority.AssemblyManager;

        private Dictionary<Type, List<Type>> m_AttributeMap;

        public async UniTask Init()
        {
            m_AttributeMap = new Dictionary<Type, List<Type>>();

            foreach (var assembly in m_Assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    var objects = type.GetCustomAttributes(typeof(GameAttribute), false);
                    foreach (var attribute in objects)
                    {
                        Type at = attribute.GetType();

                        if (!m_AttributeMap.TryGetValue(at, out List<Type> list))
                        {
                            list = new List<Type>();
                            m_AttributeMap.Add(at, list);
                        }

                        list.Add(type);
                    }
                }
            }

            await UniTask.Yield();
        }

        public Type[] GetTypes<T>() where T : GameAttribute 
        {
            Type type = typeof(T);
            if (m_AttributeMap.ContainsKey(type)) 
            {
                return m_AttributeMap[type].ToArray();
            }
            else
            {
                return new Type[0];
            }
        }
    }
}