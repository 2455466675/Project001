namespace Game.GSystem
{
    [Event]
    public class Backpack_InventoryAddHandler : EventBase<InventoryAddEventArgs>
    {
        public override void Invoke(InventoryAddEventArgs arg)
        {
            var items = arg.items;
            Game.System.BackpackSystem.OnAdd(items);
        }
    }

    [Event]
    public class Backpack_InventoryUpdateHandler : EventBase<InventoryUpdateEventArgs>
    {
        public override void Invoke(InventoryUpdateEventArgs arg)
        {
            var items = arg.items;
            Game.System.BackpackSystem.OnUpdate(items);
        }
    }

    [Event]
    public class Backpack_InventoryRemoveHandler : EventBase<InventoryRemoveEventArgs>
    {
        public override void Invoke(InventoryRemoveEventArgs arg)
        {
            var items = arg.items;
            Game.System.BackpackSystem.OnRemove(items);
        }
    }
}