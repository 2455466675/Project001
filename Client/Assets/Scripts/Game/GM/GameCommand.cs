using Config;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public struct CurrentMenuChanged 
    {
        public GM_Menu menu;
    }

    public enum GM_Menu_Type 
    { 
        All    = 0,
        Normal = 1,
        Other  = 2,
    }

    public class GM_Item 
    {
        public GmCfg Cfg { get; private set; }

        public GM_Item(GmCfg cfg) 
        {
            this.Cfg = cfg;
        }
    }

    public class GM_Menu 
    {
        public GM_Menu_Type Type { get; private set; }
        private List<GM_Item> items;        

        public GM_Menu(GM_Menu_Type type) 
        {
            Type = type;
            items = new List<GM_Item>();
        }

        public void Add(GM_Item item) 
        {
            items.Add(item);
        }

        public GM_Item[] GetItems() 
        {
            return items.ToArray();
        }
    }

    public class GameCommand
    {
        private List<GM_Menu> menus;

        private GM_Menu current;

        public void Init() 
        {
            menus = new List<GM_Menu>();

            Dictionary<GM_Menu_Type, GM_Menu> tempMap = new Dictionary<GM_Menu_Type, GM_Menu>();
            GM_Menu all = null;

            foreach (GM_Menu_Type menuType in Enum.GetValues(typeof(GM_Menu_Type)))
            {
                GM_Menu menu = new GM_Menu(menuType);
                menus.Add(menu);
                tempMap.Add(menuType, menu);
                if (menuType == GM_Menu_Type.All) 
                {
                    all = menu;
                }
            }

            GmCfg[] cfgs = Game.Config.FindAll<GmCfg>();
            for (int i = 0; i < cfgs.Length; i++)
            {
                GmCfg cfg = cfgs[i];
                if (tempMap.TryGetValue((GM_Menu_Type)cfg.Class, out GM_Menu menu)) 
                {
                    GM_Item item = new GM_Item(cfg);                    
                    menu.Add(item);
                    all?.Add(item);
                }
            }
        }

        public GM_Menu[] GetMenus() 
        {
            return menus.ToArray();
        }

        public void Select(GM_Menu menu) 
        {
            current = menu;
            Game.Event.Publish(new CurrentMenuChanged() { menu = current });
        }
    }
}