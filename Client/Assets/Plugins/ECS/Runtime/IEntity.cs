namespace ECS
{
    internal interface IEntity : IAddComponent, IGetComponent
    {
        public int Eid { get; }

        internal void Init(int eid, IWorld componentFactory);
        internal void Destroy();      
    }
}
