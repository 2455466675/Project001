using MVC;

namespace Game.System
{
    /// <summary>
    /// ÓÎÏ·½ÇÉ«Àà
    /// </summary>
	public class RoleBase : DataProxy
    {
        public Actor Actor { get; protected set; }

        public RoleBase(DataContainer container) : base(container)
        {
                    
        }

        public virtual void RefreshActor()
        {

        }
    }
}

