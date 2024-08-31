using Game.Cfg;
using Game.Core;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewMainMenuItemDB : ItemDB
    {
        public int id;
        public IntDB name;
        public OverviewMenuCfg cfg;

        public OverviewMainMenuItemDB(OverviewMenuCfg cfg)
        {
            id = cfg.Id;
            name = new IntDB(cfg.Name);
            this.cfg = cfg;
        }

        public override int CompareTo(ItemDB db)
        {
            if (db is OverviewMainMenuItemDB menu)
            {
                return id.CompareTo(menu.id);
            }
            else
            {
                return 0;
            }
        }
    }
}

