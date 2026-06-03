using System;

namespace ECS
{
    internal interface IWorld
    {
        T CreateComponent<T>(IEntity entity) where T : IComponent, new();

        IComponent CreateComponent(Type type);

        void DestroyComponent(IComponent component);

        Entity GetEntity(int eid);
    }
}