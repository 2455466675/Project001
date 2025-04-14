using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleSystem
    {
        public ActorContainer Container { get; private set; }

        private int uidGenerator;
        private Dictionary<int, RoleUnit> units;

        private HashSet<IFixedUpdateComponent> fixedUpdateComponents;
        private List<IFixedUpdateComponent> newfixedUpdateComponents;
        private List<IFixedUpdateComponent> oldfixedUpdateComponents;

        public void Init(GameInitConfig config)
        {
            uidGenerator = 10000;
            units = new Dictionary<int, RoleUnit>();
            fixedUpdateComponents = new HashSet<IFixedUpdateComponent>();
            newfixedUpdateComponents = new List<IFixedUpdateComponent>();
            oldfixedUpdateComponents = new List<IFixedUpdateComponent>();

            var go = Game.Resource.LoadAndInstantiate(config.ActorContainerPath, Game.Root.transform);
            Container = go.GetComponent<ActorContainer>();        
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

        public T CreateUnit<T>() where T : RoleUnit, new()
        {
            int uid = ++uidGenerator;
            T unit = new();
            unit.Constructor(this, uid);
            units.Add(uid, unit);
            return unit;
        }

        public void DestroyUnit(int uid) 
        {            
            if (!units.ContainsKey(uid)) 
            {
                return;
            }

            units.Remove(uid);
        }

        public T CreateComponent<T>(RoleUnit unit) where T : UnitComponent, new()
        {
            T component = new();            
            component.Constructor(unit);
            if (component is IAwakeComponent a) 
            {
                a.Awake();
            }
            if (component is IFixedUpdateComponent u) 
            {
                newfixedUpdateComponents.Add(u);
            }
            return component;
        }

        public void DestroyComponent(UnitComponent component) 
        {
            if (component == null)
            {
                return;
            }

            if (component is not IFixedUpdateComponent u) 
            {
                return;
            }
            component.Destroy();
            oldfixedUpdateComponents.Add(u);
        }
    }
}
