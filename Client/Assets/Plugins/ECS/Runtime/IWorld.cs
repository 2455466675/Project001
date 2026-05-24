namespace ECS
{
    internal interface IWorld
    {
        T CreateComponent<T>(IEntity entity) where T : IComponent, new();
        void DestroyComponent(IComponent component);
    }
}