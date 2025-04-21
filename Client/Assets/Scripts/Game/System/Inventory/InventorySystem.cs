using Config;
using System.Collections.Generic;
using System.Linq;

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

    public struct InventoryItemData 
    {
        public int id;
        public int count;
    }

    public struct InventoryItemData2
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

        public void Init() 
        {
            items = new Dictionary<long, InventoryItem>();
        }

        public InventoryItem GetItem(long uid) 
        {
            if (items.ContainsKey(uid))
            {
                return items[uid];
            }
            else
            {
                return null;
            }
        }

        public void Increment(InventoryItemData[] items) 
        {
            List<long> add = new List<long>();
            List<long> update = new List<long>();

            foreach (var item in items)
            {
                ItemChangedResult result = IncrementItem(item.id, item.count);
                if (result.type == ItemChangedType.Add) 
                {
                    add.Add(result.uid);
                }
                else if (result.type == ItemChangedType.Update)
                {
                    update.Add(result.uid);
                }
            }

            add = add.Distinct().ToList();
            update = update.Distinct().ToList();

            if (add.Count > 0) 
            {
                Game.Event.Publish(new InventoryAdd() { items = add.ToArray() });
            }

            if (update.Count > 0)
            {
                Game.Event.Publish(new InventoryUpdate() { items = update.ToArray() });
            }
        }

        public void Decrement(InventoryItemData2[] items) 
        {
            List<long> remove = new List<long>();

            foreach (var item in items)
            {
                ItemChangedResult result = DecrementItem(item.uid, item.count);
                if (result.type == ItemChangedType.Remove)
                {
                    remove.Add(result.uid);
                }
            }

            remove = remove.Distinct().ToList();

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
            long uid = isHeap ? id : Common.GenerateUid();

            if (isHeap && items.ContainsKey(uid))
            {
                result.type = ItemChangedType.Update;
                InventoryItem item = items[uid];
                item.UpdateCount(count);
            }
            else
            {
                result.type = ItemChangedType.Add;
                InventoryItem item = new InventoryItem(uid, cfg, count);
                items.Add(uid, item);
            }

            result.uid = uid;
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
            result.uid = uid;
            result.type = item.Count > count ? ItemChangedType.Update : ItemChangedType.Remove;
            item.UpdateCount(count *= -1);

            return result;
        }

        public void Test() 
        {
            InventoryItemData[] test1 = new InventoryItemData[20];
            for (int i = 0; i < 20; i++)
            {
                test1[i] = new InventoryItemData()
                {
                    id = 200001,
                    count = 1,
                };
            }
            Increment(test1);

            InventoryItemData[] test2 = new InventoryItemData[10];
            for (int i = 0; i < 10; i++)
            {
                test2[i] = new InventoryItemData()
                {
                    id = 200002,
                    count = 1,
                };
            }
            Increment(test2);
        }
    }
}