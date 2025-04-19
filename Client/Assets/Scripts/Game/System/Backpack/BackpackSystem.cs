using Config;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game.System
{
    public enum BackpackCompartmentType
    {
        New = 999,
        All = 998,
        Normal = InventoryItemType.Normal,
        Weapon = InventoryItemType.Weapon,
        Helmet = InventoryItemType.Helmet,
        Armour = InventoryItemType.Armour,
        Trouser = InventoryItemType.Trouser,
        Shoe = InventoryItemType.Shoe,
        Accessory = InventoryItemType.Accessory,
        Core = InventoryItemType.Core,
    }

    public class BackpackSystem
    {
        private Dictionary<BackpackCompartmentType, BackpackCompartment> compartments;

        public void Init() 
        {
            compartments = new Dictionary<BackpackCompartmentType, BackpackCompartment>();

            BackpackCfg[] cfgs = Game.Config.FindAll<BackpackCfg>();
            foreach (var cfg in cfgs)
            {
                BackpackCompartmentType type = (BackpackCompartmentType)cfg.Type;
                compartments.Add(type, new BackpackCompartment(cfg));
            }
        }

        public BackpackCompartment[] GetCompartments()
        {
            return compartments.Values.ToArray();
        }

        public void OnSelectCompartment(BackpackCompartment compartment)
        {
            
        }
    }
}