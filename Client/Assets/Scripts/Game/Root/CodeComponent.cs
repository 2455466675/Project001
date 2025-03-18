using ECS;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game
{
    public class CodeComponent : Entity
    {
        private Dictionary<Type, List<Type>> allTypes;

        public void Init() 
        {
            allTypes = new Dictionary<Type, List<Type>>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            foreach (var type in types)
            {
                object[] objects = type.GetCustomAttributes(typeof(GameAttribute), false);
                foreach (object attr in objects)
                {
                    Type at = attr.GetType();
                    if (!allTypes.ContainsKey(at)) 
                    {
                        allTypes.Add(at, new List<Type>());
                    }
                    allTypes[at].Add(type);
                }
            }
        }

        public List<Type> GetTypes<T>() where T : GameAttribute
        {
            Type type = typeof(T);
            if (allTypes.ContainsKey(type)) 
            {
                return allTypes[type];
            }
            else
            {
                return new List<Type>();
            }
        }
    }
}