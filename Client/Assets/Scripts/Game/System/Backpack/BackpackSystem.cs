using Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.System
{
    public struct BackpackItemsChangedArg
    {
        public BackpackItem[] items;
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

        private BackpackCompartment currentCompartment;

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

        public void OnAdd(InventoryItemData3[] datas) 
        {
            List<BackpackCompartmentType> changedTyps = new List<BackpackCompartmentType>();
            changedTyps.Add(BackpackCompartmentType.All);
            changedTyps.Add(BackpackCompartmentType.New);

            for (int i = 0; i < datas.Length; i++)
            {
                long uid = datas[i].uid;
                var item = Game.System.InventorySystem.GetItem(uid);
                if (item == null)
                {
                    MLog.Error($"item is null : {uid}");
                    continue;
                }

                var compartment = GetBackpackCompartment(item.Type);
                if (compartment == null)
                {
                    MLog.Error($"compartment is null : {item.Type}");
                    continue;
                }

                compartment.Add(item);
                allCompartment.Add(item);
                newCompartment.Add(item);

                changedTyps.Add(compartment.Type);
            }

            OnItemsChanged(changedTyps);
        }

        public void OnUpdate(InventoryItemData3[] datas) 
        {
            for (int i = 0; i < datas.Length; i++) 
            {
                long uid = datas[i].uid;
                var item = Game.System.InventorySystem.GetItem(uid);
                if (item == null)
                {
                    MLog.Error($"item is null : {uid}");
                    continue;
                }
                var compartment = GetBackpackCompartment(item.Type);
                if (compartment == null)
                {
                    MLog.Error($"compartment is null : {item.Type}");
                    continue;
                }

                compartment.Update(uid);
                allCompartment.Update(uid);
                newCompartment.Update(uid);
            }
        }

        public void OnRemove(InventoryItemData3[] datas) 
        {
            List<BackpackCompartmentType> changedTyps = new List<BackpackCompartmentType>();
            changedTyps.Add(BackpackCompartmentType.All);
            changedTyps.Add(BackpackCompartmentType.New);

            for (int i = 0; i < datas.Length; i++) 
            {
                long uid = datas[i].uid;
                int id = datas[i].id;

                ItemCfg cfg = Game.Config.Find<ItemCfg>(id);
                if (cfg == null)
                {
                    MLog.Error($"ItemCfg is null : {id}");
                    continue;
                }

                var compartment = GetBackpackCompartment((BackpackCompartmentType)cfg.Backpack);
                if (compartment == null)
                {
                    MLog.Error($"compartment is null : {cfg.Backpack}");
                    continue;
                }

                compartment.Remove(uid);
                allCompartment.Remove(uid);
                newCompartment.Remove(uid);

                changedTyps.Add(compartment.Type);
            }

            OnItemsChanged(changedTyps);
        }

        public BackpackCompartment[] GetCompartments()
        {
            return compartments.Values.ToArray();
        }

        public void OnSelectCompartment(BackpackCompartment compartment)
        {
            currentCompartment = compartment;            
            PublishItemsChanged();
        }

        private void OnItemsChanged(List<BackpackCompartmentType> compartmentTypes)
        {
            if (currentCompartment == null) 
            {
                return;
            }

            if (compartmentTypes == null || compartmentTypes.Count == 0) 
            {
                return;
            }

            if (compartmentTypes.Contains(currentCompartment.Type)) 
            {
                PublishItemsChanged();
            }
        }

        private void PublishItemsChanged() 
        {
            if (currentCompartment == null) 
            {
                return;
            }
            Game.Event.Publish(new BackpackItemsChangedArg() { items = currentCompartment.GetItems() });
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