using Game.Cfg;
using Game.Core;

namespace Game
{
    public class Item 
    { 
        public int Id { get; private set; }
        public int CreateTime { get; private set; }
        public int Count { get; private set; }
        public ItemCfg Cfg { get; private set; }

        public Item(int id) 
        { 
            Id = id;
            Cfg = GameCore.GameCfgData.Find<ItemCfg>(id);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ItemSystem
    {
        public ItemSystem() 
        { 
            
        }
    }
}