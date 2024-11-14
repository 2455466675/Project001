using MVC;
using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class TeamSystem : DataProxy, IGameSystem
    {
        private List<SceneRole> characters;

        public TeamSystem(DataContainer container) : base(container)
        {
        }
    }
}

