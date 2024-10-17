using Game.Cfg;
using MVC;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewSystem : DataProxy, IGameSystem
	{
        public OverviewSystem(DataContainer container) : base(container)
        { 
            var list = GameCore.Cfg.FindAll<OverviewMenuCfg>();

            DataCollection menuList = CreateCollection("MenuList");
            for (int i = 0; i < 6; i++)
            {
                DataContainer item = menuList.Append(true);
                item.SetBaseValue("id", 910005 + i);
            }
        }
    }
}

