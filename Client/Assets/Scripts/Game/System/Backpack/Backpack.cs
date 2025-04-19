using Config;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
    public class Backpack
    {
        public InventoryItemType Type { get; private set; }
        public BackpackCfg Cfg { get; private set; }

        private HashSet<InventoryItem> items;

        public Backpack(BackpackCfg cfg) 
        {
            this.Cfg = cfg;
            this.Type = (InventoryItemType)cfg.Type;
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

        public InventoryItem[] GetItems() 
        {
            return items.ToArray();
        }
    }
}
