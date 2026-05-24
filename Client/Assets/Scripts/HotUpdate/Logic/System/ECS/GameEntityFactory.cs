using ECS;
using System;
using GameFramework.Core;

namespace GameFramework.Logic
{
    [GameSystem]
    public class GameEntityFactory : IGameSystem, IInit, IUpdateable, IFixedUpdateable, ILateUpdateable
    {
        private World world;

        void IInit.Init()
        {
            world = new World();
            world.Init();
        }

        void IUpdateable.Update(float deltaTime)
        {
            world.Update(deltaTime);
        }

        void IFixedUpdateable.FixedUpdate(float fixedDeltaTime)
        {
            world.FixedUpdate(fixedDeltaTime);
        }

        void ILateUpdateable.LateUpdate(float deltaTime)
        {
            world.LateUpdate(deltaTime);
        }

        public Entity CreateEntity()
        {
            return world.CreateEntity();
        }

        public Entity CreateEntity(params Type[] components)
        {
            return world.CreateEntity(components);
        }

        public Entity CreateEntity<T>() where T : ComponentBase, new()
        {
            return world.CreateEntity<T>();
        }

        public Entity CreateEntity<T0, T1>() where T0 : ComponentBase, new() where T1 : ComponentBase, new()
        {
            return world.CreateEntity<T0, T1>();
        }

        public Entity CreateEntity<T0, T1, T2>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new()
        {
            return world.CreateEntity<T0, T1, T2>();
        }

        public Entity CreateEntity<T0, T1, T2, T3>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new() where T3 : ComponentBase, new()
        {
            return world.CreateEntity<T0, T1, T2, T3>();
        }

        public Entity CreateEntity<T0, T1, T2, T3, T4>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new() where T3 : ComponentBase, new() where T4 : ComponentBase, new()
        {
            return world.CreateEntity<T0, T1, T2, T3, T4>();
        }

        public void DestroyEntity(Entity entity)
        {
            world.DestroyEntity(entity);
        }

        public void DestroyEntity(int eid)
        {
            world.DestroyEntity(eid);
        }
    }
}