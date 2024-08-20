using Game.Cfg;
using Game.Core;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewMainMenuItem : ItemDB
    {
        public int id;
        public IntDB name = new IntDB();

        public OverviewMenuCfg cfg;

        public OverviewMainMenuItem(OverviewMenuCfg cfg)
        {
            id = cfg.Id;
            name.Value = cfg.Name;
            this.cfg = cfg;
        }

        public override int CompareTo(ItemDB db)
        {
            if (db is OverviewMainMenuItem menu)
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

