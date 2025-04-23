namespace Game.System
{
    [Event]
    public class Backpack_InventoryAddHandler : EventBase<InventoryAdd>
    {
        public override void Invoke(InventoryAdd arg)
        {
            var items = arg.items;
            Game.System.BackpackSystem.OnAdd(items);
        }
    }

    [Event]
    public class Backpack_InventoryUpdateHandler : EventBase<InventoryUpdate>
    {
        public override void Invoke(InventoryUpdate arg)
        {
            var items = arg.items;
            Game.System.BackpackSystem.OnUpdate(items);
        }
    }

    [Event]
    public class Backpack_InventoryRemoveHandler : EventBase<InventoryRemove>
    {
        public override void Invoke(InventoryRemove arg)
        {
            var items = arg.items;
            Game.System.BackpackSystem.OnRemove(items);
        }
    }
}