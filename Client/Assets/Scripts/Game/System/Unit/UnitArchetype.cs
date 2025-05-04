using System.Collections.Generic;

namespace Game.System
{
    public abstract class UnitArchetypeBase : UnitBase
    {
        internal void Invoke(List<UnitComponent> components) 
        {
            for (int i = 0; i < components.Count; i++)
            {
                UnitComponent component = components[i];
                if (component is IAwakeComponent ac)
                {
                    ac.Awake();
                }
            }
        }
    }

    public abstract class UnitArchetype : UnitArchetypeBase
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {          
        }
    }

    public abstract class UnitArchetype<C> : UnitArchetypeBase
        where C : UnitComponent, new()
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {
            List<UnitComponent> components = new()
            {
                AddComponentInner<C>(true)
            };
            Invoke(components);
        }
    }

    public abstract class UnitArchetype<C1, C2> : UnitArchetypeBase
        where C1 : UnitComponent, new() 
        where C2 : UnitComponent, new()
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {
            List<UnitComponent> components = new()
            {
                AddComponentInner<C1>(true),
                AddComponentInner<C2>(true),
            };
            Invoke(components);
        }
    }

    public abstract class UnitArchetype<C1, C2, C3> : UnitArchetypeBase
        where C1 : UnitComponent, new()
        where C2 : UnitComponent, new()
        where C3 : UnitComponent, new()
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {
            List<UnitComponent> components = new()
            {
                AddComponentInner<C1>(true),
                AddComponentInner<C2>(true),
                AddComponentInner<C3>(true),
            };
            Invoke(components);
        }
    }

    public abstract class UnitArchetype<C1, C2, C3, C4> : UnitArchetypeBase
        where C1 : UnitComponent, new()
        where C2 : UnitComponent, new()
        where C3 : UnitComponent, new()
        where C4 : UnitComponent, new()
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {
            List<UnitComponent> components = new()
            {
                AddComponentInner<C1>(true),
                AddComponentInner<C2>(true),
                AddComponentInner<C3>(true),
                AddComponentInner<C4>(true),
            };
            Invoke(components);
        }
    }

    public abstract class UnitArchetype<C1, C2, C3, C4, C5> : UnitArchetypeBase
        where C1 : UnitComponent, new()
        where C2 : UnitComponent, new()
        where C3 : UnitComponent, new()
        where C4 : UnitComponent, new()
        where C5 : UnitComponent, new()
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {
            List<UnitComponent> components = new()
            {
                AddComponentInner<C1>(true),
                AddComponentInner<C2>(true),
                AddComponentInner<C3>(true),
                AddComponentInner<C4>(true),
                AddComponentInner<C5>(true),
            };
            Invoke(components);
        }
    }

    public abstract class UnitArchetype<C1, C2, C3, C4, C5, C6> : UnitArchetypeBase
        where C1 : UnitComponent, new()
        where C2 : UnitComponent, new()
        where C3 : UnitComponent, new()
        where C4 : UnitComponent, new()
        where C5 : UnitComponent, new()
        where C6 : UnitComponent, new()
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {
            List<UnitComponent> components = new()
            {
                AddComponentInner<C1>(true),
                AddComponentInner<C2>(true),
                AddComponentInner<C3>(true),
                AddComponentInner<C4>(true),
                AddComponentInner<C5>(true),
                AddComponentInner<C6>(true),
            };
            Invoke(components);
        }
    }

    public abstract class UnitArchetype<C1, C2, C3, C4, C5, C6, C7> : UnitArchetypeBase
        where C1 : UnitComponent, new()
        where C2 : UnitComponent, new()
        where C3 : UnitComponent, new()
        where C4 : UnitComponent, new()
        where C5 : UnitComponent, new()
        where C6 : UnitComponent, new()
        where C7 : UnitComponent, new()
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {
            List<UnitComponent> components = new()
            {
                AddComponentInner<C1>(true),
                AddComponentInner<C2>(true),
                AddComponentInner<C3>(true),
                AddComponentInner<C4>(true),
                AddComponentInner<C5>(true),
                AddComponentInner<C6>(true),
                AddComponentInner<C7>(true),
            };
            Invoke(components);
        }
    }

    public abstract class UnitArchetype<C1, C2, C3, C4, C5, C6, C7, C8> : UnitArchetypeBase
        where C1 : UnitComponent, new()
        where C2 : UnitComponent, new()
        where C3 : UnitComponent, new()
        where C4 : UnitComponent, new()
        where C5 : UnitComponent, new()
        where C6 : UnitComponent, new()
        where C7 : UnitComponent, new()
        where C8 : UnitComponent, new()
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {
            List<UnitComponent> components = new()
            {
                AddComponentInner<C1>(true),
                AddComponentInner<C2>(true),
                AddComponentInner<C3>(true),
                AddComponentInner<C4>(true),
                AddComponentInner<C5>(true),
                AddComponentInner<C6>(true),
                AddComponentInner<C7>(true),
                AddComponentInner<C8>(true),
            };
            Invoke(components);
        }
    }

    public abstract class UnitArchetype<C1, C2, C3, C4, C5, C6, C7, C8, C9> : UnitArchetypeBase
        where C1 : UnitComponent, new()
        where C2 : UnitComponent, new()
        where C3 : UnitComponent, new()
        where C4 : UnitComponent, new()
        where C5 : UnitComponent, new()
        where C6 : UnitComponent, new()
        where C7 : UnitComponent, new()
        where C8 : UnitComponent, new()
        where C9 : UnitComponent, new()
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {
            List<UnitComponent> components = new()
            {
                AddComponentInner<C1>(true),
                AddComponentInner<C2>(true),
                AddComponentInner<C3>(true),
                AddComponentInner<C4>(true),
                AddComponentInner<C5>(true),
                AddComponentInner<C6>(true),
                AddComponentInner<C7>(true),
                AddComponentInner<C8>(true),
                AddComponentInner<C9>(true),
            };
            Invoke(components);
        }
    }

    public abstract class UnitArchetype<C1, C2, C3, C4, C5, C6, C7, C8, C9, C10> : UnitArchetypeBase
        where C1 : UnitComponent, new()
        where C2 : UnitComponent, new()
        where C3 : UnitComponent, new()
        where C4 : UnitComponent, new()
        where C5 : UnitComponent, new()
        where C6 : UnitComponent, new()
        where C7 : UnitComponent, new()
        where C8 : UnitComponent, new()
        where C9 : UnitComponent, new()
        where C10 : UnitComponent, new()
    {
        internal override void InitArchetype(UnitManager unitSystem)
        {
            List<UnitComponent> components = new()
            {
                AddComponentInner<C1>(true),
                AddComponentInner<C2>(true),
                AddComponentInner<C3>(true),
                AddComponentInner<C4>(true),
                AddComponentInner<C5>(true),
                AddComponentInner<C6>(true),
                AddComponentInner<C7>(true),
                AddComponentInner<C8>(true),
                AddComponentInner<C9>(true),
                AddComponentInner<C10>(true),
            };
            Invoke(components);
        }
    }
}
