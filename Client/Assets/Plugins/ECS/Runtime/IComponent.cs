namespace ECS
{
    internal interface IComponent : IGetComponent
    {
        public int Eid { get; }
        internal void Init(IEntity entity);

        internal void Destroy();
    }
}