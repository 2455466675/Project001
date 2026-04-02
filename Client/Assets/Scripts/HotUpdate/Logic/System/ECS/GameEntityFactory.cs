using ECS;
using System;
using GameFramework.Core;

namespace GameFramework.Logic
{
    [GameSystem]
    public class GameEntityFactory : IGameSystem, IInit, IUpdateable, IFixedUpdateable, ILateUpdateable
    {
        private EntityManager entityManager;

        void IInit.Init()
        {
            entityManager = new EntityManager();
            entityManager.Init();
        }

        void IUpdateable.Update(float deltaTime)
        {
            entityManager.Update(deltaTime);
        }

        void IFixedUpdateable.FixedUpdate(float fixedDeltaTime)
        {
            entityManager.FixedUpdate(fixedDeltaTime);
        }

        void ILateUpdateable.LateUpdate(float deltaTime)
        {
            entityManager.LateUpdate(deltaTime);
        }

        public Entity CreateEntity()
        {
            return entityManager.CreateEntity();
        }

        public Entity CreateEntity(params Type[] components)
        {
            return entityManager.CreateEntity(components);
        }

        public Entity CreateEntity<T>() where T : ComponentBase, new()
        {
            return entityManager.CreateEntity<T>();
        }

        public Entity CreateEntity<T0, T1>() where T0 : ComponentBase, new() where T1 : ComponentBase, new()
        {
            return entityManager.CreateEntity<T0, T1>();
        }

        public Entity CreateEntity<T0, T1, T2>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new()
        {
            return entityManager.CreateEntity<T0, T1, T2>();
        }

        public Entity CreateEntity<T0, T1, T2, T3>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new() where T3 : ComponentBase, new()
        {
            return entityManager.CreateEntity<T0, T1, T2, T3>();
        }

        public Entity CreateEntity<T0, T1, T2, T3, T4>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new() where T3 : ComponentBase, new() where T4 : ComponentBase, new()
        {
            return entityManager.CreateEntity<T0, T1, T2, T3, T4>();
        }
    }
}