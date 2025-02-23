using System.Collections.Generic;

namespace EC
{
    /// <summary>
    /// 
    /// </summary>
    public class ComponentManager
    {
        private int index;

        private HashSet<IUpdate> updateableComponents;
        private HashSet<ILateUpdate> lateUpdateableComponents;

        private List<Component> destroyedComponents;

        internal ComponentManager() 
        {
            index = 20000;
            updateableComponents = new HashSet<IUpdate>();
            lateUpdateableComponents = new HashSet<ILateUpdate>();
            destroyedComponents = new List<Component>();
        }

        internal void Update(float dt) 
        {
            TickComponent(dt);
            DestroyComponentInner();
        }

        internal T CreateComponent<T>(Entity entity) where T : Component, new() 
        {
            T component = new();
            component.Initialize(entity, GenerateGuid());

            if (component is IAwake ac)
            {
                ac.Awake();
            }
            if (component is IStart sc)
            {
                sc.Start();
            }
            if (component is IUpdate uc)
            {
                updateableComponents.Add(uc);
            }
            if (component is ILateUpdate luc)
            {
                lateUpdateableComponents.Add(luc);
            }

            return component;
        }

        internal void DestroyComponent(Component component) 
        {
            destroyedComponents.Add(component);
        }

        private void TickComponent(float dt) 
        {
            foreach (var component in updateableComponents)
            {
                component.Update(dt);
            }

            foreach (var component in lateUpdateableComponents)
            {
                component.LateUpdate(dt);
            }
        }

        private void DestroyComponentInner()
        {
            if (destroyedComponents.Count == 0) return;

            for (int i = 0; i < destroyedComponents.Count; i++)
            {
                Component component = destroyedComponents[i];
                if (component is IUpdate uc)
                {
                    updateableComponents.Remove(uc);
                }
                if (component is ILateUpdate luc)
                {
                    lateUpdateableComponents.Remove(luc);
                }
                component.Destroy();
            }

            destroyedComponents.Clear();
        }

        private int GenerateGuid()
        {
            index++;
            return index;
        }
    }
}
