namespace Game.System
{
    [Event]
    public class Backpack_InventoryAddHandler : EventBase<InventoryAdd>
    {
        public override void Invoke(InventoryAdd arg)
        {
            long[] items = arg.items;
            for (int i = 0; i < items.Length; i++) 
            {
                Game.System.BackpackSystem.OnAdd(items[i]);
            }
        }
    }

    [Event]
    public class Backpack_InventoryUpdateHandler : EventBase<InventoryUpdate>
    {
        public override void Invoke(InventoryUpdate arg)
        {
            long[] items = arg.items;
            for (int i = 0; i < items.Length; i++)
            {
                Game.System.BackpackSystem.OnUpdate(items[i]);
            }
        }
    }

    [Event]
    public class Backpack_InventoryRemoveHandler : EventBase<InventoryRemove>
    {
        public override void Invoke(InventoryRemove arg)
        {
            long[] items = arg.items;
            for (int i = 0; i < items.Length; i++)
            {
                Game.System.BackpackSystem.OnRemove(items[i]);
            }
        }
    }
}