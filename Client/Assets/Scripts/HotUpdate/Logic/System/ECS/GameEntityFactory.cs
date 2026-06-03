using ECS;
using GameFramework.Core;
using System;

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
            world.CreatedEntity += OnCreatedEntity;
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

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        public Entity CreateEntity()
        {
            var entity = world.CreateEntity();
            return entity;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        public Entity CreateEntity(params Type[] components)
        {
            var entity = world.CreateEntity(components);
            return entity;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        public Entity CreateEntity<T>() where T : ComponentBase, new()
        {
            var entity = world.CreateEntity<T>();
            return entity;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        public Entity CreateEntity<T0, T1>() where T0 : ComponentBase, new() where T1 : ComponentBase, new()
        {
            var entity = world.CreateEntity<T0, T1>();
            return entity;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        public Entity CreateEntity<T0, T1, T2>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new()
        {
            var entity = world.CreateEntity<T0, T1, T2>();
            return entity;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        public Entity CreateEntity<T0, T1, T2, T3>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new() where T3 : ComponentBase, new()
        {
            var entity = world.CreateEntity<T0, T1, T2, T3>();
            return entity;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        public Entity CreateEntity<T0, T1, T2, T3, T4>() where T0 : ComponentBase, new() where T1 : ComponentBase, new() where T2 : ComponentBase, new() where T3 : ComponentBase, new() where T4 : ComponentBase, new()
        {
            var entity = world.CreateEntity<T0, T1, T2, T3, T4>();
            return entity;
        }

        public Entity GetEntity(int eid)
        {
            return world.GetEntity(eid);
        }

        public void DestroyEntity(Entity entity)
        {
            if (entity == null)
            {
                return;
            }
            DestroyEntity(entity.Eid);
        }

        public void DestroyEntity(int eid)
        {
            world.DestroyEntity(eid);
            Game.Message.SendMessage(new DestroyEntityMessage() { eid = eid });
        }

        private void OnCreatedEntity(int eid)
        {
            Game.Message.SendMessage(new CreateEntityMessage() { eid = eid });
        }
    }
}