using System;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleUnit
    {        
        private Dictionary<Type, UnitComponent> components;

        public RoleUnit() 
        {
            components = new Dictionary<Type, UnitComponent>();
        }

        public T AddComponent<T>() where T : UnitComponent, new()
        {
            Type type = typeof(T);
            if (components.ContainsKey(type)) 
            {
                return components[type] as T;
            }
            else
            {
                T component = new();
                components.Add(type, component);
                return component;
            }                   
        }

        public T GetComponent<T>() where T : UnitComponent 
        {
            Type type = typeof(T);
            if (components.ContainsKey(type)) 
            {
                return components[type] as T;
            }
            else
            {
                return default;
            }
        }

        public void RemoveComponent<T>() where T : UnitComponent 
        {
            Type type = typeof(T);
            if (!components.ContainsKey(type))
            {
                return;
            }
            components.Remove(type);
        }
    }
}