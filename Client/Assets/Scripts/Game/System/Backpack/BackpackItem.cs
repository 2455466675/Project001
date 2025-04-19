using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    public class BackpackItem
    {
        public InventoryItem Item { get; private set; }

        public BackpackItem(InventoryItem item)
        {
            Item = item;
        }
    }
}