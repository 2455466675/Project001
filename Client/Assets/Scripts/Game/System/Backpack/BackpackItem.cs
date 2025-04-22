namespace Game.System
{
    public class BackpackItem : GameNavigationItemData
    {
        public InventoryItem Item { get; private set; }

        public BackpackItem(InventoryItem item)
        {
            Item = item;
        }
    }
}