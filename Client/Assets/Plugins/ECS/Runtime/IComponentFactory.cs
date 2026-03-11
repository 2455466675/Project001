namespace ECS
{
    internal interface IComponentFactory
    {
        T CreateComponent<T>(IEntity entity) where T : IComponent, new();
        void DestroyComponent(IComponent component);
    }
}