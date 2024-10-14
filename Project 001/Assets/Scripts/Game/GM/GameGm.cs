
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
        private Dictionary<int, Action<object>> cmds;

        public GameGm() 
        {
            List<GmCfg> cfgList = GameCore.GameCfg.FindAll<GmCfg>();


 

            InitCmds();
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

