using Config;
using System.Collections.Generic;
using System.Linq;

namespace Game.System
{
    public struct InventoryAdd
    {
        public InventoryItemData3[] items;
    }

    public struct InventoryUpdate
    {
        public InventoryItemData3[] items;
    }

    public struct InventoryRemove
    {
        public InventoryItemData3[] items;
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

    public struct InventoryItemData3
    {
        public int id;
        public long uid;        
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
            public int id;
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
            List<InventoryItemData3> add = new List<InventoryItemData3>();
            List<InventoryItemData3> update = new List<InventoryItemData3>();

            foreach (var item in items)
            {
                ItemChangedResult result = IncrementItem(item.id, item.count);
                if (result.type == ItemChangedType.Add) 
                {
                    add.Add(new InventoryItemData3() { id = result.id, uid = result.uid });
                }
                else if (result.type == ItemChangedType.Update)
                {
                    update.Add(new InventoryItemData3() { id = result.id, uid = result.uid });
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

        public void Decrement(InventoryItemData2[] items) 
        {
            List<InventoryItemData3> remove = new List<InventoryItemData3>();
            List<InventoryItemData3> update = new List<InventoryItemData3>();

            foreach (var item in items)
            {
                ItemChangedResult result = DecrementItem(item.uid, item.count);
                if (result.type == ItemChangedType.Remove)
                {
                    remove.Add(new InventoryItemData3() { id = result.id, uid = result.uid });
                }
                else if (result.type == ItemChangedType.Update)
                {
                    update.Add(new InventoryItemData3() { id = result.id, uid = result.uid });
                }
            }

            if (update.Count > 0)
            {
                Game.Event.Publish(new InventoryUpdate() { items = update.ToArray() });
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

            result.id = id;
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
            result.id = item.Id;
            result.uid = uid;
         
            if (item.Count > count)
            {
                item.UpdateCount(count *= -1);
                result.type = ItemChangedType.Update;
            }
            else
            {
                result.type = ItemChangedType.Remove;
                items.Remove(uid);
            }

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