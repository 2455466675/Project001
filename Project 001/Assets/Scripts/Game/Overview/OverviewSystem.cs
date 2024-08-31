
using Game.Cfg;
using Game.Core;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewSystem : IGameSystem
	{
        private ListDB<OverviewMainMenuItemDB> menus;

        public OverviewSystem()
        { 
            menus = new ListDB<OverviewMainMenuItemDB>();

            var list = GameCore.GameCfg.FindAll<OverviewMenuCfg>();
            for (int i = 0; i < list.Count; i++)
            {
                menus.Add(new OverviewMainMenuItemDB(list[i]));
            }
        }

        public IDataBase GetMenus()
        {
            return menus;
        }
    }
}

