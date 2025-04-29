using System;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class UnitBase
    {
        public int Uid { get; private set; }
        private UnitManager unitSystem;
        private Dictionary<Type, UnitComponent> components;

        internal void InitUnit(UnitManager unitSystem, int uid) 
        {
            this.Uid = uid;
            this.unitSystem = unitSystem;
            components = new Dictionary<Type, UnitComponent>();
            InitArchetype(unitSystem);
            OnInitUnit();
        }

        internal void Destroy()
        {
            OnDestroyUnit();

            foreach (var component in components.Values)
            {
                unitSystem.DestroyComponent(component);
            }
            components.Clear();
            unitSystem = null;
        }

        internal T AddComponentInner<T>(bool isSilent) where T : UnitComponent, new()
        {
            Type type = typeof(T);
            if (components.ContainsKey(type))
            {
                return components[type] as T;
            }
            else
            {
                T component = unitSystem.CreateComponent<T>(this, isSilent);
                components.Add(type, component);
                return component;
            }
        }

        internal virtual void InitArchetype(UnitManager unitSystem)
        {

        }

        public T AddComponent<T>() where T : UnitComponent, new()
        {
            return AddComponentInner<T>(false);
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

        protected virtual void OnInitUnit()
        {
        }

        protected virtual void OnDestroyUnit() 
        {
        }
    }
}