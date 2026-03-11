namespace ECS
{
    internal interface IGetComponent
    {
        public T GetComponent<T>() where T : class, IComponent;
    }
}