using Config;
using System.Collections.Generic;
using System.Linq;

namespace Game.System
{
    public struct InventoryDataChanged
    {
                
    }

    public struct SelectBackpackMenu
    {
        public Backpack backpack;
    }

    public struct ItemBuffer 
    {
        public long uid;
        public int id;
        public int deltaCount;
    }

    public class InventorySystem
    {
        private Dictionary<long, InventoryItem> items;
        private Dictionary<InventoryItemType, Backpack> backpacks;

        public void Init() 
        {
            items = new Dictionary<long, InventoryItem>();
            backpacks = new Dictionary<InventoryItemType, Backpack>();

            BackpackCfg[] cfgs = Game.Config.FindAll<BackpackCfg>();

            foreach (var cfg in cfgs)
            {
                InventoryItemType type = (InventoryItemType)cfg.Type;
                backpacks.Add(type, new Backpack(cfg));
            }

            for (int i = 0; i < 20; i++)
            {
                UpdateItem(new ItemBuffer() { id = 200001 , uid = Common.GenerateUid(), deltaCount = 1});                
            }

            for (int i = 0; i < 10; i++)
            {
                UpdateItem(new ItemBuffer() { id = 200002, uid = Common.GenerateUid(), deltaCount = 1 });
            }
        }

        public void IncrementItem(int id, int count) 
        {
            if (count <= 0) 
            {
                return;
            }

            ItemCfg cfg = Game.Config.Find<ItemCfg>(id);
            if (cfg == null)
            {
                MLog.Error($"ItemCfg is null : {id}");
                return;
            }

            bool isHeap = cfg.Heap;
            long key = isHeap ? id : Common.GenerateUid();

            if (isHeap && items.ContainsKey(key))
            {
                InventoryItem item = items[key];
                item.UpdateCount(count);
            }
            else
            {
                InventoryItem item = new InventoryItem(cfg, count);
                items.Add(key, item);
            }
        }

        public void DecrementItem(long uid, int count) 
        {
            if (!items.ContainsKey(uid))
            {
                return;
            }
                
            count = GameMathf.Abs(count);

            InventoryItem item = items[uid];           
            if (item.Count > count) 
            {
                item.UpdateCount(count *= -1);            
            }
            else
            {
                items.Remove(uid);                
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
                    items.Remove(key);
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
                    //buffer.deltaCount = isHeap ? deltaCount : 1;
                    //InventoryItem item = new InventoryItem(buffer, cfg);
                    //items.Add(key, item);
                }
            }         
        }

        public Backpack[] GetBackpacks() 
        {
            return backpacks.Values.ToArray();
        }    
        
        public void OnSelectBackpack(Backpack backpack) 
        {
            Game.Event.Publish(new SelectBackpackMenu() { backpack = backpack });
        }
    }
}