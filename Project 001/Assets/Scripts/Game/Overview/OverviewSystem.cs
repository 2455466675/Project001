
using Game.Cfg;
using Game.Core;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewSystem : IGameSystem
	{
        public OverviewSystem()
        { 
            var list = GameCore.GameCfg.FindAll<OverviewMenuCfg>();
        }
    }
}

