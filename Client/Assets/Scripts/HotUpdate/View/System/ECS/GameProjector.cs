using ECS;
using GameFramework.Core;
using GameFramework.Logic;
using System;
using System.Collections.Generic;

namespace GameFramework.View
{
    /// <summary>
    /// 游戏对象在视图层的投影
    /// </summary>
    [GameSystem]
    public class GameProjector : IGameSystem, IInit, IUpdateable, IFixedUpdateable, ILateUpdateable
    {
        private readonly static Dictionary<Type, Type> ComponetProjectionMap = new Dictionary<Type, Type>() 
        {
            [typeof(ActorComponent)] = typeof(ActorProjectionComponent),
            [typeof(MotorComponent)] = typeof(MotorProjectionComponent),
        };

        private World world;
        private Dictionary<int, int> projections;

        void IInit.Init()
        {
            world = new World();
            world.Init();
            projections = new Dictionary<int, int>();
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
        /// 创建投影
        /// </summary>
        /// <param name="eid">逻辑层实体Eid</param>
        public void CreateProjection(int eid)
        {
            if (projections.ContainsKey(eid))
            {
                MDebug.Warn($"实体投影已存在，本体：{eid}， 投影：{projections[eid]}");
                return;
            }

            var entity = world.CreateEntity();
            var pjc = entity.AddComponent<ProjectionComponent>();
            pjc.SetHost(eid);
            projections.Add(eid, entity.Eid);
        }

        public void DestroyProjection(int eid)
        {
            if (!projections.ContainsKey(eid))
            {
                return;
            }

            world.DestroyEntity(projections[eid]);
            projections.Remove(eid);
        }

        public void ProjectComponent(int hostEid, Type hostType)
        {
            if (hostType == null)
            {
                return;
            }

            if (!ComponetProjectionMap.TryGetValue(hostType, out var projectionType))
            {
                MDebug.Error($"没有对应的投影组件：{hostType.FullName}");
                return;
            }

            Entity projectionEntity = GetProjection(hostEid);
            if (projectionEntity == null)
            {
                return;
            }

            var projectionCom = projectionEntity.AddComponent(projectionType);
            var hostEntity = Game.GetSystem<GameEntityFactory>().GetEntity(hostEid);
            var hostCom = hostEntity.GetComponent(hostType);

            IProjectable projectable = (IProjectable)hostCom;
            IProjection projection = (IProjection)projectionCom;
            projectable.Reflect(projection);            

        }

        public Entity GetProjection(int hostEid)
        {
            projections.TryGetValue(hostEid, out var projectionEid);
            return world.GetEntity(projectionEid);
        }
    }
}