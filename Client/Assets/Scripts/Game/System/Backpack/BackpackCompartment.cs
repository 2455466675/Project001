using Config;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game.System
{ 
    public class BackpackCompartment
    {
        public BackpackCompartmentType Type;

        public BackpackCfg Cfg { get; private set; }

        private HashSet<BackpackItem> items;

        public BackpackCompartment(BackpackCfg cfg)
        {
            this.Cfg = cfg;
            this.Type = (BackpackCompartmentType)cfg.Type;
            items = new HashSet<BackpackItem>();
        }

        public void Add(InventoryItem item)
        {
            BackpackItem backpackItem = new BackpackItem(item);
            items.Add(backpackItem);
        }

        public void Remove(InventoryItem item)
        {
            //items.Remove(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public BackpackItem[] GetItems()
        {
            return items.ToArray();
        }
    }
}