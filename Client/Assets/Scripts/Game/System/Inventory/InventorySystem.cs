using Config;
using System.Collections.Generic;

namespace Game.System
{
    public struct InventoryAdd
    {
        public long[] items;
    }

    public struct InventoryUpdate
    {
        public long[] items;
    }

    public struct InventoryRemove
    {
        public long[] items;
    }

    public struct ItemBuffer 
    {
        public int id;
        public int count;
    }

    public struct ItemBuffer2
    {
        public long uid;
        public int count;
    }

    public class InventorySystem
    {
        public enum ItemChangedType
        {
            Error,
            Add,
            Update,
            Remove,
        }

        public struct ItemChangedResult
        {
            public ItemChangedType type;
            public long uid;
        }

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

            //for (int i = 0; i < 20; i++)
            //{
            //    UpdateItem(new ItemBuffer() { id = 200001 , uid = Common.GenerateUid(), deltaCount = 1});                
            //}

            //for (int i = 0; i < 10; i++)
            //{
            //    UpdateItem(new ItemBuffer() { id = 200002, uid = Common.GenerateUid(), deltaCount = 1 });
            //}
        }

        public void Increment(ItemBuffer[] items) 
        {
            Dictionary<int, int> temp = new Dictionary<int, int>();

            for (int i = 0; i < items.Length; i++) 
            {
                ItemBuffer item = items[i];
                int id = item.id;
                int count = item.count;
                if (temp.ContainsKey(id)) 
                {
                    temp[id] += count;
                }
                else
                {
                    temp[id] = count;
                }
            }

            List<long> add = new List<long>();
            List<long> update = new List<long>();

            foreach (var item in temp)
            {
                ItemChangedResult result = IncrementItem(item.Key, item.Value);
                if (result.type == ItemChangedType.Add) 
                {
                    add.Add(result.uid);
                }
                else if (result.type == ItemChangedType.Update)
                {
                    update.Add(result.uid);
                }
            }

            if (add.Count > 0) 
            {
                Game.Event.Publish(new InventoryAdd() { items = add.ToArray() });
            }

            if (update.Count > 0)
            {
                Game.Event.Publish(new InventoryUpdate() { items = update.ToArray() });
            }
        }

        public void Decrement(ItemBuffer2[] items) 
        {
            Dictionary<long, int> temp = new Dictionary<long, int>();

            for (int i = 0; i < items.Length; i++)
            {
                ItemBuffer2 item = items[i];
                long uid = item.uid;
                int count = GameMathf.Abs(item.count);
                if (temp.ContainsKey(uid))
                {
                    temp[uid] += count;
                }
                else
                {
                    temp[uid] = count;
                }
            }

            List<long> remove = new List<long>();

            foreach (var item in temp)
            {
                ItemChangedResult result = DecrementItem(item.Key, item.Value);
                if (result.type == ItemChangedType.Remove)
                {
                    remove.Add(result.uid);
                }
            }

            if (remove.Count > 0) 
            {
                Game.Event.Publish(new InventoryRemove() { items = remove.ToArray() });
            }
        }

        private ItemChangedResult IncrementItem(int id, int count) 
        {
            ItemChangedResult result = new ItemChangedResult();
            if (count <= 0) 
            {
                result.type = ItemChangedType.Error;
                return result;
            }

            ItemCfg cfg = Game.Config.Find<ItemCfg>(id);
            if (cfg == null)
            {
                MLog.Error($"ItemCfg is null : {id}");
                result.type = ItemChangedType.Error;
                return result;
            }

            bool isHeap = cfg.Heap;
            long key = isHeap ? id : Common.GenerateUid();

            if (isHeap && items.ContainsKey(key))
            {
                result.type = ItemChangedType.Update;
                InventoryItem item = items[key];
                item.UpdateCount(count);
            }
            else
            {
                result.type = ItemChangedType.Add;
                InventoryItem item = new InventoryItem(cfg, count);
                items.Add(key, item);
            }

            result.uid = key;
            return result;
        }

        private ItemChangedResult DecrementItem(long uid, int count) 
        {
            ItemChangedResult result = new ItemChangedResult();
            if (!items.ContainsKey(uid))
            {
                result.type = ItemChangedType.Error;
                return result;
            }
                
            count = GameMathf.Abs(count);

            InventoryItem item = items[uid];           
            if (item.Count > count) 
            {
                result.type = ItemChangedType.Update;
                item.UpdateCount(count *= -1);            
            }
            else
            {
                result.type = ItemChangedType.Remove;
                items.Remove(uid);                
            }

            result.uid = uid;
            return result;
        }
    }
}