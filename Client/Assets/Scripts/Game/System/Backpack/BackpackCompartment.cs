using Config;
using System.Collections.Generic;
using System.Linq;

namespace Game.System
{ 
    public class BackpackCompartment : GameNavigationItemData
    {
        public BackpackCompartmentType Type;

        public BackpackCfg Cfg { get; private set; }

        private Dictionary<long, BackpackItem> items;

        public BackpackCompartment(BackpackCfg cfg)
        {
            this.Cfg = cfg;
            this.Type = (BackpackCompartmentType)cfg.Type;
            items = new Dictionary<long, BackpackItem>();
        }

        public void Add(InventoryItem item)
        {
            BackpackItem backpackItem = new BackpackItem(item);
            items.Add(item.Uid, backpackItem);
        }

        public void Update(long uid) 
        {
            if (items.TryGetValue(uid, out BackpackItem backpackItem)) 
            {
                backpackItem.Refresh();
            }
        }

        public void Remove(long uid)
        {
            items.Remove(uid);
        }

        public void Clear()
        {
            items.Clear();
        }

        public BackpackItem[] GetItems()
        {
            return items.Values.ToArray();
        }
    }
}