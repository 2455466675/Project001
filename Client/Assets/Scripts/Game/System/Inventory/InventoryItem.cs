using Config;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>

    public class InventoryItem
    {
        public int Id => config.Id;
        public int Count => count;
        public ItemCfg Config => config;

        private int count;
        private ItemCfg config;

        public InventoryItem(ItemBuffer buffer, ItemCfg cfg)
        {
            count = buffer.deltaCount;
            config = cfg;
        }

        public void Update(ItemBuffer buffer)
        {
            count += buffer.deltaCount;
        }
    }
}
