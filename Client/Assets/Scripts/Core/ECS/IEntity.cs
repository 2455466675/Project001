namespace ECS
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntity
    {
        public int Guid { get; }

        public T AddComponent<T>() where T : Component, new();
        public T GetComponent<T>() where T : Component;
        public void RemoveComponent<T>() where T : Component;
    }
}
