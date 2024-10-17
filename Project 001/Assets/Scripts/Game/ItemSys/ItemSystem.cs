using Game.Cfg;
using MVC;

namespace Game.System
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
            Cfg = GameCore.Cfg.Find<ItemCfg>(id);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ItemSystem : DataProxy, IGameSystem
    {
        public PackageSystem Package { get; private set; }

        public ItemSystem(DataContainer container) : base(container) 
        {            
            Package = new PackageSystem(CreateContainer("Package"));
        }
    }
}