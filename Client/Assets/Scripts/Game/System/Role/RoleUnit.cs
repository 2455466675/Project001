using System;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleUnit
    {
        public int Uid { get; private set; }
        private RoleSystem roleSystem;
        private Dictionary<Type, UnitComponent> components;

        public void Constructor(RoleSystem roleSystem, int uid) 
        {
            this.Uid = uid;
            this.roleSystem = roleSystem;
            components = new Dictionary<Type, UnitComponent>();
            OnConstructor();
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
                T component = roleSystem.CreateComponent<T>(this);
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

        public void Destroy() 
        {
            foreach (var component in components.Values) 
            {
                roleSystem.DestroyComponent(component);
            }
            components.Clear();
            roleSystem.DestroyUnit(this.Uid);
            roleSystem = null;
        }

        protected virtual void OnConstructor()
        {
        }
    }
}