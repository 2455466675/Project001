using Config;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Game.System.InventorySystem;

namespace Game.System
{
    public struct OnSelectBackpackCompartmentArg
    {
        public BackpackCompartment compartment;
    }

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

        private BackpackCompartment allCompartment;
        private BackpackCompartment newCompartment;

        public void Init() 
        {
            compartments = new Dictionary<BackpackCompartmentType, BackpackCompartment>();

            BackpackCfg[] cfgs = Game.Config.FindAll<BackpackCfg>();
            foreach (var cfg in cfgs)
            {
                BackpackCompartmentType type = (BackpackCompartmentType)cfg.Type;
                BackpackCompartment compartment = new BackpackCompartment(cfg);
                compartments.Add(type, compartment);
                if (type == BackpackCompartmentType.All) 
                {
                    allCompartment = compartment;
                }
                if (type == BackpackCompartmentType.New) 
                { 
                    newCompartment = compartment;
                }
            }
        }

        public void OnAdd(long uid) 
        {
            MLog.Log($"OnAdd : {uid}");
            var item = Game.System.InventorySystem.GetItem(uid);
            if (item == null)
            {
                MLog.Error($"item is null : {uid}");
                return;
            }

            var compartment = GetBackpackCompartment(item.Type);
            if (compartment == null) 
            {
                MLog.Error($"compartment is null : {item.Type}");
                return;
            }

            compartment.Add(item);
            allCompartment.Add(item);
            newCompartment.Add(item);
        }

        public void OnUpdate(long uid) 
        {
            MLog.Log($"OnUpdate : {uid}");
            var item = Game.System.InventorySystem.GetItem(uid);
            if (item == null)
            {
                MLog.Error($"item is null : {uid}");
                return;
            }
            var compartment = GetBackpackCompartment(item.Type);
            if (compartment == null)
            {
                MLog.Error($"compartment is null : {item.Type}");
                return;
            }

            compartment.Update(uid);
        }

        public void OnRemove(long uid, int id) 
        {
            MLog.Log($"OnRemove : {uid}, {id}");
            ItemCfg cfg = Game.Config.Find<ItemCfg>(id);
            if (cfg == null)
            {
                MLog.Error($"ItemCfg is null : {id}");
                return;
            }

            var compartment = GetBackpackCompartment((BackpackCompartmentType)cfg.Backpack);
            if (compartment == null)
            {
                MLog.Error($"compartment is null : {cfg.Backpack}");
                return;
            }

            compartment.Remove(uid);
            allCompartment.Remove(uid);
            newCompartment.Remove(uid);
        }

        public BackpackCompartment[] GetCompartments()
        {
            return compartments.Values.ToArray();
        }

        public void OnSelectCompartment(BackpackCompartment compartment)
        {
            Game.Event.Publish(new OnSelectBackpackCompartmentArg() { compartment = compartment});
        }

        private BackpackCompartment GetBackpackCompartment(InventoryItemType type)
        {
            BackpackCompartmentType t = (BackpackCompartmentType)type;
            if (compartments.ContainsKey(t))
            {
                return compartments[t];
            }
            else
            {
                return null;
            }
        }

        private BackpackCompartment GetBackpackCompartment(BackpackCompartmentType type) 
        {
            if (compartments.ContainsKey(type)) 
            {
                return compartments[type];
            }
            else
            {
                return null;
            }
        }
    }
}