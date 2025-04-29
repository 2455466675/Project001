using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitManager
    {
        private int uidGenerator;
        private Dictionary<int, UnitBase> units;

        private HashSet<IFixedUpdateComponent> fixedUpdateComponents;
        private List<IFixedUpdateComponent> newfixedUpdateComponents;
        private List<IFixedUpdateComponent> oldfixedUpdateComponents;

        public void Init()
        {
            uidGenerator = 10000;
            units = new Dictionary<int, UnitBase>();
            fixedUpdateComponents = new HashSet<IFixedUpdateComponent>();
            newfixedUpdateComponents = new List<IFixedUpdateComponent>();
            oldfixedUpdateComponents = new List<IFixedUpdateComponent>();    
        }

        public void FixedUpdate(float fdt) 
        {
            if (newfixedUpdateComponents.Count > 0)
            {
                for (int i = 0; i < newfixedUpdateComponents.Count; i++)
                {
                    fixedUpdateComponents.Add(newfixedUpdateComponents[i]);
                }
                newfixedUpdateComponents.Clear();
            }

            if (oldfixedUpdateComponents.Count > 0)
            {
                for (int i = 0; i < oldfixedUpdateComponents.Count; i++)
                {
                    fixedUpdateComponents.Remove(oldfixedUpdateComponents[i]);
                }
                oldfixedUpdateComponents.Clear();
            }

            foreach (var component in fixedUpdateComponents)
            {
                component.FixedUpdate(fdt);
            }
        }

        public T CreateUnit<T>() where T : UnitBase, new()
        {
            int uid = ++uidGenerator;
            T unit = new();
            unit.InitUnit(this, uid);
            units.Add(uid, unit);
            return unit;
        }

        public void DestroyUnit(int uid) 
        {            
            if (!units.ContainsKey(uid)) 
            {
                return;
            }

            UnitBase unit = units[uid];
            unit.Destroy();
            units.Remove(uid);
        }

        internal T CreateComponent<T>(UnitBase unit, bool isSilent) where T : UnitComponent, new()
        {
            T component = new();            
            component.InitComponent(unit);

            if (!isSilent) 
            {
                if (component is IAwakeComponent a) 
                {
                    a.Awake();
                }            
            }

            if (component is IFixedUpdateComponent u) 
            {
                newfixedUpdateComponents.Add(u);
            }
            return component;
        }

        internal void DestroyComponent(UnitComponent component) 
        {
            if (component == null)
            {
                return;
            }

            component.Destroy();

            if (component is IFixedUpdateComponent u) 
            {
                oldfixedUpdateComponents.Add(u);                
            }
        }
    }
}
