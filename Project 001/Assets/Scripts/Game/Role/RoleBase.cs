using MVC;

namespace Game.System
{
    /// <summary>
    /// сно╥╫ги╚
    /// </summary>
	public class RoleBase : DataProxy
    {
        public int Id { get; protected set; }
        public Actor Actor { get; protected set; }

        public RoleBase(DataContainer container) : base(container)
        {
                    
        }

        public virtual void RefreshActor()
        {

        }
    }
}

