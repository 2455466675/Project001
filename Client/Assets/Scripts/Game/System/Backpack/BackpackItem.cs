using Navigation;

namespace Game.System
{
    public class BackpackItem : NavigationItemData
    {
        public InventoryItem Item { get; private set; }

        public BackpackItem(InventoryItem item)
        {
            Item = item;
        }
    }
}