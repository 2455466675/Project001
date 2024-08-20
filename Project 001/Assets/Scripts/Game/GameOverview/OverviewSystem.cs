
using Game.Cfg;
using Game.Core;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewSystem : IGameSystem
	{
        private ListDB<OverviewMainMenuItem> menus;

        public OverviewSystem()
        { 
            menus = new ListDB<OverviewMainMenuItem>();

            var list = GameCore.GameCfg.FindAll<OverviewMenuCfg>();
            for (int i = 0; i < list.Count; i++)
            {
                menus.Add(new OverviewMainMenuItem(list[i]));
            }
        }

        public IDataBase GetMenus()
        {
            return menus;
        }
    }
}

