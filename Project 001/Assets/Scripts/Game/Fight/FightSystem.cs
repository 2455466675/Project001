using MVC;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class FightSystem : DataProxy, IGameSystem
    {
        public FightSystem(DataContainer container) : base(container)
        {
        }
    }
}

