using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ECS
{
    public class EntityManager : IComponentFactory
    {
        private int eidGenerator;
        private Dictionary<int, IEntity> entites;

        private Dictionary<Type, Func<IComponent>> componentFactories;
        private HashSet<IUpdateableComponent> updateableComponents;
        private HashSet<IUpdateableComponent> addUpdateableComponents;
        private HashSet<IUpdateableComponent> remUpdateableComponents;
        private HashSet<IFixedUpdateableComponent> fixedUpdateableComponents;
        private HashSet<IFixedUpdateableComponent> addfixedUpdateableComponents;
        private HashSet<IFixedUpdateableComponent> remfixedUpdateableComponents;

        public void Init()
        {
            eidGenerator = 100000;
            entites = new Dictionary<int, IEntity>();
            componentFactories = new Dictionary<Type, Func<IComponent>>();
            updateableComponents = new HashSet<IUpdateableComponent>();
            addUpdateableComponents = new HashSet<IUpdateableComponent>();
            remUpdateableComponents = new HashSet<IUpdateableComponent>();
            fixedUpdateableComponents = new HashSet<IFixedUpdateableComponent>();
            addfixedUpdateableComponents = new HashSet<IFixedUpdateableComponent>();
            remfixedUpdateableComponents = new HashSet<IFixedUpdateableComponent>();
        }

        public void Update(float deltaTime)
        {
            if (addUpdateableComponents.Count > 0)
            {
                foreach (var item in addUpdateableComponents)
                {
                    updateableComponents.Add(item);
                }
                addUpdateableComponents.Clear();
            }

            foreach (var item in updateableComponents)
            {
                item.Update(deltaTime);
            }

            if (remUpdateableComponents.Count > 0)
            {
                foreach (var item in remUpdateableComponents)
                {
                    updateableComponents.Remove(item);
                }
                remUpdateableComponents.Clear();
            }
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            if (addfixedUpdateableComponents.Count > 0)
            {
                foreach (var item in addfixedUpdateableComponents)
                {
                    fixedUpdateableComponents.Add(item);
                }
                addfixedUpdateableComponents.Clear();
            }

            foreach (var item in fixedUpdateableComponents)
            {
                item.FixedUpdate(fixedDeltaTime);
            }

            if (remfixedUpdateableComponents.Count > 0)
            {
                foreach (var item in remfixedUpdateableComponents)
                {
                    fixedUpdateableComponents.Remove(item);
                }
                remfixedUpdateableComponents.Clear();
            }
        }

        #region CreateEntity

        public Entity CreateEntity()
        {
            int eid = eidGenerator++;
            IEntity entity = new Entity();
            entity.Init(eid, this);
            entites.Add(eid, entity);
            return (Entity)entity;
        }

        public Entity CreateEntity(params Type[] components)
        {
            Entity entity = CreateEntity();

            int length = components.Length;
            IComponent[] temps = new IComponent[length];
            for (int i = 0; i < length; i++)
            {
                IComponent c = CreateComponent(components[i]);
                entity.AddComponent(c);
                temps[i] = c;
            }
            for (int i = 0; i < length; i++)
            {
                temps[i].Init(entity);
            }

            return entity;
        }

        public Entity CreateEntity<T>() where T : ComponentBase, new()
        {
            Entity entity = CreateEntity();

            IComponent c = CreateComponent<T>();
            entity.AddComponent(c);
            c.Init(entity);

            return entity;
        }

        public Entity CreateEntity<T0, T1>() where T0 : ComponentBase, new() where T1 : ComponentBase, new()
        {
            Entity entity = CreateEntity();

            IComponent c0 = CreateComponent<T0>();
            IComponent c1 = CreateComponent<T1>();
            entity.AddComponent(c0);
            entity.AddComponent(c1);
            c0.Init(entity);
            c1.Init(entity);

            return entity;
        }

        public Entity CreateEntity<T0, T1, T2>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new()
        {
            Entity entity = CreateEntity();

            IComponent c0 = CreateComponent<T0>();
            IComponent c1 = CreateComponent<T1>();
            IComponent c2 = CreateComponent<T2>();
            entity.AddComponent(c0);
            entity.AddComponent(c1);
            entity.AddComponent(c2);
            c0.Init(entity);
            c1.Init(entity);
            c2.Init(entity);

            return entity;
        }

        public Entity CreateEntity<T0, T1, T2, T3>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new() where T3 : ComponentBase, new()
        {
            Entity entity = CreateEntity();

            IComponent c0 = CreateComponent<T0>();
            IComponent c1 = CreateComponent<T1>();
            IComponent c2 = CreateComponent<T2>();
            IComponent c3 = CreateComponent<T3>();
            entity.AddComponent(c0);
            entity.AddComponent(c1);
            entity.AddComponent(c2);
            entity.AddComponent(c3);
            c0.Init(entity);
            c1.Init(entity);
            c2.Init(entity);
            c3.Init(entity);

            return entity;
        }

        public Entity CreateEntity<T0, T1, T2, T3, T4>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new() where T3 : ComponentBase, new() where T4 : ComponentBase, new()
        {
            Entity entity = CreateEntity();

            IComponent c0 = CreateComponent<T0>();
            IComponent c1 = CreateComponent<T1>();
            IComponent c2 = CreateComponent<T2>();
            IComponent c3 = CreateComponent<T3>();
            IComponent c4 = CreateComponent<T4>();
            entity.AddComponent(c0);
            entity.AddComponent(c1);
            entity.AddComponent(c2);
            entity.AddComponent(c3);
            entity.AddComponent(c4);
            c0.Init(entity);
            c1.Init(entity);
            c2.Init(entity);
            c3.Init(entity);
            c4.Init(entity);

            return entity;
        }

        #endregion

        public void DestroyEntity(int eid)
        {
            if (!entites.ContainsKey(eid))
            {
                return;
            }

            IEntity entity = entites[eid];
            entity.Destroy();
            entites.Remove(eid);
        }

        T IComponentFactory.CreateComponent<T>(IEntity entity)
        {
            T component = CreateComponent<T>();
            component.Init(entity);
            return component;
        }

        void IComponentFactory.DestroyComponent(IComponent component)
        {
            UnregisterComponent(component);
            component.Destroy();
        }

        private T CreateComponent<T>() where T : IComponent, new()
        {
            T component = new T();
            RegisterComponent(component);
            return component;
        }

        private IComponent CreateComponent(Type componentType)
        {
            if (!componentFactories.TryGetValue(componentType, out var factory))
            {
                var constructor = componentType.GetConstructor(Type.EmptyTypes);
                var newExp = Expression.New(constructor);
                var lambda = Expression.Lambda<Func<IComponent>>(newExp);
                factory = lambda.Compile();

                componentFactories[componentType] = factory;
            }

            IComponent component = factory();
            RegisterComponent(component);
            return component;
        }

        private void RegisterComponent(IComponent component)
        {
            if (component is IUpdateableComponent update)
            {
                addUpdateableComponents.Add(update);
            }
            if (component is IFixedUpdateableComponent fixedUpdate)
            {
                addfixedUpdateableComponents.Add(fixedUpdate);
            }
        }

        private void UnregisterComponent(IComponent component)
        {
            if (component is IUpdateableComponent update)
            {
                remUpdateableComponents.Add(update);
            }
            if (component is IFixedUpdateableComponent fixedUpdate)
            {
                remfixedUpdateableComponents.Add(fixedUpdate);
            }
        }
    }
}