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

        public InventoryItem(ItemCfg config, int count)
        {
            this.count = count;
            this.config = config;
        }

        public void UpdateCount(int deltaCount) 
        {
            count += deltaCount;
        }

        public void Update(ItemBuffer buffer)
        {
            count += buffer.deltaCount;
        }
    }
}
