using Game.Cfg;
using Game.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class WinOverviewController : UIController
	{
        public StaticListView itemTypeList;
        public LoopListView itemList;

        TeamSystem teamSystem;
        
        private void Start()
        {
            teamSystem = new TeamSystem();
            ListDB<PackageItemDB> items = teamSystem.roles[0].items;
            
            itemList.SetDatum(items);

            ListDB<MenuItem> menuList = new ListDB<MenuItem>();
            menuList.Add(new MenuItem(1, "道具"));
            menuList.Add(new MenuItem(2, "属性"));
            menuList.Add(new MenuItem(3, "技能"));

            defaultListView.SetDatum(menuList);

            ListDB<ItemTypeItemDB> typeList = new ListDB<ItemTypeItemDB>();
            typeList.Add(new ItemTypeItemDB(0, "全部"));
            typeList.Add(new ItemTypeItemDB(1, "药品"));
            typeList.Add(new ItemTypeItemDB(2, "武器"));
            typeList.Add(new ItemTypeItemDB(3, "防具"));
            typeList.Add(new ItemTypeItemDB(4, "任务"));

            itemTypeList.SetDatum(typeList);
        }

        public void Update()
        {
            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    MLog.Log("KeyCode.R");
            //}
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    MLog.Log("KeyCode.Q");
            //    ListDB items = teamSystem.roles[0].items;
            //    int index = UnityEngine.Random.Range(0, items.Count);
            //    ItemDB itemDB = items[index];
            //    (itemDB as PackageItemDB).count.Value = 0;
            //    items.NotifyChange();
            //}
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    MLog.Log("KeyCode.T");
            //    List<Role> roleList = teamSystem.roles;
            //    for (int i = 0; i < roleList.Count; i++)
            //    {
            //        roleList[i].hp.Value = 111;
            //    }
            //}
            //if (Input.GetKeyDown(KeyCode.U))
            //{
            //    MLog.Log("KeyCode.T");
            //    List<Role> roleList = teamSystem.roles;
            //    for (int i = 0; i < roleList.Count; i++)
            //    {
            //        roleList[i].name.Value = "vsews";
            //    }
            //}
        }

        public void OnTest(UINotification notification)
        {
            MLog.Log("OnTest");
            GameCore.UI.SelectListView(itemTypeList);            
        }

        public void OnTest2(UINotification notification)
        {
            MLog.Log("OnTest2");
            GameCore.UI.SelectListView(itemList);            
        }

        public void OnTest3(UINotification notification)
        {
            ItemTypeItemDB db = notification.ListItem.GetListItem<ItemTypeItemDB>();
            MLog.Log($"OnTest3:{db.name}, {db.id}");

            ListDB<PackageItemDB> items = teamSystem.roles[0].items;            
            PackageItemDB.select = (PakageItemType)db.id.Value;

            items.NotifyChange();
        }

        public void OnSubmit(UINotification notification) 
        {
            string text = notification.ListItem.CurrentGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            MLog.Log($"OnSubmit:{text}");
        }

        public void OnSelect(UINotification notification)
        {
            string text = notification.ListItem.CurrentGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            MLog.Log($"OnSelect:{text}");
        }

        public void OnMoveLeft(UINotification notification)
        {
            string text = notification.ListItem.CurrentGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            MLog.Log($"OnMoveLeft:{text}");
        }

        public void OnMoveRight(UINotification notification)
        {
            string text = notification.ListItem.CurrentGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            MLog.Log($"OnMoveRight:{text}");
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

    public class MenuItem : ItemDB
    {
        public IntDB id = new IntDB();
        public StringDB name = new StringDB();

        public MenuItem(int id, string name)
        {
            this.id.Value = id;
            this.name.Value = name;
        }

        public override int CompareTo(ItemDB db)
        {
            if (db is MenuItem menu)
            {
                return id.CompareTo(menu.id);
            }
            else
            {                
                return 0;
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
        None    = 0,
        Drug    = 1,
        Weapon  = 2,
        Armor   = 3,
        Task    = 4,
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

