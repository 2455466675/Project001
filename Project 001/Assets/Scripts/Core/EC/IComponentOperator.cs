namespace EC
{
    /// <summary>
    /// 
    /// </summary>
    public interface IComponentOperator
    {
        T GetComponent<T>() where T : Component;
        T AddComponent<T>() where T : Component, new();
        void RemoveComponent<T>();
    }
}