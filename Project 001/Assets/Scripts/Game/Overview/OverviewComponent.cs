using EC;
using Game.Cfg;
using Game.Core;
using System.Collections.Generic;

namespace Game.System
{
    public class OverviewMenuItem 
    {
        public OverviewMenuCfg cfg;
    }

    /// <summary>
    /// 
    /// </summary>
    public class OverviewComponent : Component, IAwake
    {
        private List<OverviewMenuItem> menuList;

        public void Awake()
        {
            var list = World.GetComponent<ConfigComponent>().FindAll<OverviewMenuCfg>();

            int count = list.Count;
            menuList = new List<OverviewMenuItem>(count);

            for (int i = 0; i < count; i++)
            {
                OverviewMenuItem item = new OverviewMenuItem();
                item.cfg = list[i];

                menuList.Add(item);
            }
        }

        public List<OverviewMenuItem> GetMenuList() 
        {
            return menuList;
        }
    }
}
