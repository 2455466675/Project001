namespace ECS
{
    internal interface IEntity : IAddComponent, IGetComponent
    {
        public int Eid { get; }

        internal void Init(int eid, IComponentFactory componentFactory);
        internal void Destroy();      
    }
}
