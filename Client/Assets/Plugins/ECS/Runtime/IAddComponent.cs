namespace ECS
{
    internal interface IAddComponent
    {
        internal T AddComponent<T>() where T : IComponent, new();
    }
}
