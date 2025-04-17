using Config;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.System
{
    public enum BackpackType 
    {
        All,
        New,
        Normal,
        Weapon,
        Helmet,
        Armour,
        Trouser,
        Shoe,
        Accessory,
        Core,
    }

    public class InventoryItem
    {
        private int id;
        private int count;
        private ItemCfg config;

        public int Id => id;
        public int Count => count;
        public ItemCfg Config => config;
    }

    public class InventorySystem
    {
        private Dictionary<long, InventoryItem> items;

        public void Init() 
        {
            items = new Dictionary<long, InventoryItem>();

        }
    }
}