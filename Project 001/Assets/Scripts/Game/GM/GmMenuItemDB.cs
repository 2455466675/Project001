using Game.Cfg;
using Game.Core;
using System;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class GmMenuItemDB : ItemDB
    {
        public int id;

        public StringDB name;

        public GmCfg cfg;

        public ListDB<GmListItemDB> items;

        public GmMenuItemDB(GmCfg cfg)
        {
            id = cfg.Id;
            name = new StringDB(cfg.Name);
            this.cfg = cfg;

            string[] cfgItems = cfg.Cmds.Split(';', StringSplitOptions.RemoveEmptyEntries);

            items = new ListDB<GmListItemDB>(cfgItems.Length);

            for (int i = 0; i < cfgItems.Length; i++)
            {
                items.Add(new GmListItemDB(cfgItems[i]));   
            }
        }

        public override int CompareTo(ItemDB db)
        {
            if (db is GmMenuItemDB itemDB)
            {
                return id.CompareTo(itemDB.id);
            }
            else
            {
                return 0;
            }
        }
    }
}

