using Config;
using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class Backpack
    {
        public BackpackType Type { get; private set; }
        public BackpackCfg Cfg { get; private set; }

        private HashSet<InventoryItem> items;

        public Backpack(BackpackCfg cfg) 
        {
            this.Cfg = cfg;
            this.Type = (BackpackType)cfg.Type;
            items = new HashSet<InventoryItem>();
        }

        public void Add(InventoryItem item) 
        {
            items.Add(item);
        }

        public void Remove(InventoryItem item) 
        {
            items.Remove(item);
        }

        public void Clear() 
        {
            items.Clear();
        }
    }
}
