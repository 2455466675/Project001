
using Game.Cfg;
using MVC;
using System;
using System.Collections.Generic;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class GameGm : DataProxy, IGameSystem
	{
        private Dictionary<int, Action<object>> cmds;

        private DataCollection menuList;

        public GameGm(DataContainer container) : base(container) 
        {
            List<GmCfg> cfgList = GameCore.Cfg.FindAll<GmCfg>();

            menuList = CreateCollection("MenuList");
 
            for (int i = 0; i < cfgList.Count; i++) 
            {
                GmCfg cfg = cfgList[i];
                DataContainer item = menuList.Append();
                item.SetBaseValue("id", cfg.Id);
                item.SetBaseValue("name", cfg.Name);
                item.SetBaseValue("cmds", cfg.Cmds);

                DataCollection cmdList = item.CreateCollection("CmdList");

                string[] strings = cfg.Cmds.Split(';', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < strings.Length; j++)
                {
                    string[] strings2 = strings[j].Split(':', StringSplitOptions.RemoveEmptyEntries);

                    DataContainer cmdItem = cmdList.Append();
                    cmdItem.SetBaseValue("id", strings2[0]);
                    cmdItem.SetBaseValue("name", strings2[1]);
                    cmdItem.SetBaseValue("args", strings2.Length > 2 ? strings2[2] : string.Empty);
                }    
            }

            SetContainerLinker("Current", null);

            InitCmds();
        }

        public void SelectMenu(DataContainer menuItem)
        {
            SetContainerLinker("Current", menuItem);
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

            GameCore.Scene.LoadSceneAsync("FightScene", UnityEngine.SceneManagement.LoadSceneMode.Single, null, (s) => {

                MLog.Log("Execute Cmd1001 end");
                //GameCore.UI.Exit();
                //GameCore.UI.Enter(UI.WindowId.WinFightBg);
                //GameCore.UI.SelectNavigatable(UI.ListViewId.FightEnemyList);
            
            } );
        }

        private void Cmd1002(object o)
        {
            MLog.Log("Execute Cmd1002");
        }
    }
}

