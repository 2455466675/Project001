using Config;

namespace Game.GSystem
{
    /// <summary>
    /// 
    /// </summary>

    public class InventoryItem
    {
        public long Uid => uid;
        public int Id => config.Id;
        public int Count => count;
        public InventoryItemType Type => (InventoryItemType)config.Backpack;
        public ItemCfg Config => config;

        private long uid;
        private int count;
        private ItemCfg config;

        public InventoryItem(long uid, ItemCfg config, int count)
        {
            this.uid = uid;
            this.count = count;
            this.config = config;
        }

        public void UpdateCount(int deltaCount) 
        {
            count = GameMathf.Max(0, count + deltaCount);
        }
    }
}
