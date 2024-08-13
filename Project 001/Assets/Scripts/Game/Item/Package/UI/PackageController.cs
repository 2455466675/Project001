using Game.Cfg;
using Game.Core;
using Game.System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class PackageController : UIController
	{

        TeamSystem teamSystem;

        protected override void Register()
        {
            ListDB<ItemTypeItemDB> typeList = new ListDB<ItemTypeItemDB>();
            typeList.Add(new ItemTypeItemDB(0, "全部"));
            typeList.Add(new ItemTypeItemDB(1, "药品"));
            typeList.Add(new ItemTypeItemDB(2, "武器"));
            typeList.Add(new ItemTypeItemDB(3, "防具"));
            typeList.Add(new ItemTypeItemDB(4, "任务"));

            ListView.Register(ListViewId.PackageMenuList, typeList);

            teamSystem = new TeamSystem();
            ListDB<PackageItemDB> items = teamSystem.roles[0].items;
            ListView.Register(ListViewId.PackageItemList, items);       
        }

        public void OnTest2(UINotification notification)
        {
            GameCore.UI.SelectGuidableGroup(ListViewId.PackageItemList);
        }

        public void OnTest3(UINotification notification)
        {
            ItemTypeItemDB db = notification.ListItem.GetItemDB<ItemTypeItemDB>();
            MLog.Log($"OnTest3:{db.name}, {db.id}");

            ListDB<PackageItemDB> items = teamSystem.roles[0].items;
            PackageItemDB.select = (PakageItemType)db.id.Value;

            items.NotifyChange();
        }

        public void OnTest4(UINotification notification)
        {
            PackageItemDB db = notification.ListItem.GetItemDB<PackageItemDB>();
            MLog.Log($"OnTest4:{db.name}, {db.id}");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                ListDB<PackageItemDB> items = teamSystem.roles[0].items;
                items.RemoveAt(Random.Range(0, items.Count()));                
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                ListDB<PackageItemDB> items = teamSystem.roles[0].items;
                PakageItemType itemType = (PakageItemType)Random.Range(1, 5);
                string name = $"{itemType}类道具：{items.Count() + 1}";
                PackageItemDB item = new PackageItemDB(items.Count() + 1, name, itemType);
                items.Add(item);
            }
        }
    }

    public class TeamSystem
    {
        public List<Role> roles;

        public TeamSystem()
        {
            List<RoleCfg> r = GameCore.GameCfg.FindAll<RoleCfg>();
            if (r == null || r.Count <= 0)
            {
                return;
            }
            roles = new List<Role>(r.Count);
            for (int i = 0; i < r.Count; i++)
            {
                Role role = new()
                {
                    id = i.ToString(),
                    state = Role.RoleState.Fight,
                    cfg = r[i],
                    items = new ListDB<PackageItemDB>()
                };

                for (int j = 0; j < 20; j++)
                {
                    PakageItemType itemType = (PakageItemType)UnityEngine.Random.Range(1, 5);
                    string name = $"{itemType}类道具：{j}";
                    PackageItemDB item = new PackageItemDB(j, name, itemType);
                    role.items.Add(item);
                }

                roles.Add(role);
            }
        }
    }

    public class ItemTypeItemDB : ItemDB
    {
        public IntDB id = new IntDB();
        public StringDB name = new StringDB();

        public ItemTypeItemDB(int id, string name)
        {
            this.id.Value = id;
            this.name.Value = name;
        }

        public override int CompareTo(ItemDB db)
        {
            if (db is ItemTypeItemDB itemDB)
            {
                return id.CompareTo(itemDB.id);
            }
            else
            {
                return 0;
            }
        }
    }

    public enum PakageItemType
    {
        None = 0,
        Drug = 1,
        Weapon = 2,
        Armor = 3,
        Task = 4,
    }

    public class PackageItemDB : ItemDB
    {
        public static PakageItemType select;

        public int id;
        public PakageItemType itemType;
        public StringDB name = new StringDB();
        public IntDB count = new IntDB();
        public override bool Filter()
        {
            return (count.IntValue > 0) && (select == PakageItemType.None || itemType == select);
        }

        public PackageItemDB(int id, string name, PakageItemType itemType)
        {
            this.id = id;
            this.name.Value = name;
            this.itemType = itemType;
            count.Value = 1;
        }

        public override int CompareTo(ItemDB db)
        {
            return 0;
        }
    }

    public class Role
    {
        public enum RoleState
        {
            Idle,
            Fight
        }
        public string id;
        public RoleCfg cfg;
        public RoleState state;

        public IntDB hp;
        public StringDB name;

        public ListDB<PackageItemDB> items;
        public Role()
        {
            hp = new IntDB(2);
            name = new StringDB("sgew");
            items = new ListDB<PackageItemDB>();
        }
    }
}

