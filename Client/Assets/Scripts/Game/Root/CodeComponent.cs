using ECS;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game
{
    public class CodeComponent : Entity
    {
        private Dictionary<Type, HashSet<Type>> allTypes;

        public void Init() 
        {
            allTypes = new Dictionary<Type, HashSet<Type>>();

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
                        allTypes.Add(at, new HashSet<Type>());
                    }
                    allTypes[at].Add(type);
                }
            }
        }
    }
}