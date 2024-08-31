
using Game.Cfg;
using Game.Core;
using System;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class GameGm : IGameSystem
	{
        public ListDB<GmMenuItemDB> menus;
        public ListDB<GmListItemDB> items;

        private Dictionary<int, Action<object>> cmds;

        public GameGm() 
        {
            List<GmCfg> cfgList = GameCore.GameCfg.FindAll<GmCfg>();

            menus = new ListDB<GmMenuItemDB>(cfgList.Count);
  
            for (int i = 0; i < cfgList.Count; i++)
            {
                GmMenuItemDB db = new GmMenuItemDB(cfgList[i]);
                menus.Add(db);
            }

            items = new ListDB<GmListItemDB>();

            InitCmds();
        }

        public void SelectMenu(GmMenuItemDB db)
        {
            if (db == null)
            {
                return;
            }

            items.CopyTo(db.items);
        }

        public void ExecuteCmd(int cmdId, string args)
        {
            if (!cmds.ContainsKey(cmdId))
            {
                return;
            }
            cmds[cmdId].Invoke(args);
        }

        private void InitCmds()
        {
            cmds = new Dictionary<int, Action<object>>();
            cmds.Add(1001, Cmd1001);
            cmds.Add(1002, Cmd1002);
        }

        private void Cmd1001(object o)
        {
            MLog.Log("Execute Cmd1001");
        }

        private void Cmd1002(object o)
        {
            MLog.Log("Execute Cmd1002");
        }
    }
}

