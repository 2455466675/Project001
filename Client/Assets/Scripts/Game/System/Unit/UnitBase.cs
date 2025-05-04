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
        private List<UnitComponent> components;

        internal void InitUnit(UnitManager unitSystem, int uid) 
        {
            this.Uid = uid;
            this.unitSystem = unitSystem;
            components = new List<UnitComponent>();
            InitArchetype(unitSystem);
            OnInitUnit();
        }

        internal void Destroy()
        {
            OnDestroyUnit();

            foreach (var component in components)
            {
                unitSystem.DestroyComponent(component);
            }
            components.Clear();
            unitSystem = null;
        }

        internal T AddComponentInner<T>(bool isSilent) where T : UnitComponent, new()
        {
            T component = GetComponent<T>();

            if (component == null) 
            {
                component = unitSystem.CreateComponent<T>(this, isSilent);
                components.Add(component);
            }

            return component;
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
            for (int i = 0; i < components.Count; i++) 
            {
                UnitComponent component = components[i];
                if (component is T) 
                {
                    return component as T;
                }
            }

            return default;
        }

        protected virtual void OnInitUnit()
        {
        }

        protected virtual void OnDestroyUnit() 
        {
        }
    }
}