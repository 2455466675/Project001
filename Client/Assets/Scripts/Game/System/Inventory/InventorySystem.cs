using Config;
using System.Collections.Generic;

namespace Game.System
{
    public struct ItemBuffer 
    {
        public long uid;
        public int id;
        public int deltaCount;
    }

    public class InventorySystem
    {
        private Dictionary<long, InventoryItem> items;
        private Dictionary<BackpackType, Backpack> backpacks;

        public void Init() 
        {
            items = new Dictionary<long, InventoryItem>();
            backpacks = new Dictionary<BackpackType, Backpack>();

            BackpackCfg[] cfgs = Game.Config.FindAll<BackpackCfg>();

            foreach (var cfg in cfgs)
            {
                BackpackType type = (BackpackType)cfg.Type;
                backpacks.Add(type, new Backpack(cfg));
            }
        }

        public void UpdateItem(ItemBuffer buffer) 
        {
            int deltaCount = buffer.deltaCount;
            if (deltaCount == 0) 
            {
                return;
            }

            long uid = buffer.uid;
            int id = buffer.id;
            ItemCfg cfg = Game.Config.Find<ItemCfg>(id);
            if (cfg == null) 
            {
                MLog.Error($"ItemCfg is null : {id}");
                return;
            }

            BackpackType type = (BackpackType)cfg.Backpack;
            if (!backpacks.ContainsKey(type))
            {
                MLog.Error($"BackpackType is undefined : {cfg.Backpack}");
                return;
            }

            Backpack backpack = backpacks[type];

            bool isHeap = cfg.Heap;
            long key = isHeap ? id : uid;
            if (deltaCount < 0) 
            {               
                if (!items.ContainsKey(key))
                {
                    return;
                }

                InventoryItem item = items[key];
                item.Update(buffer);

                if (item.Count <= 0)
                {
                    items.Remove(id);
                    backpack.Remove(item);
                }
            }
            else
            {
                if (isHeap && items.ContainsKey(id))
                {
                    items[id].Update(buffer);
                }
                else
                {  
                    buffer.deltaCount = isHeap ? deltaCount : 1;
                    InventoryItem item = new InventoryItem(buffer, cfg);
                    items.Add(key, item);
                    backpack.Add(item);
                }
            }         
        }
    }
}