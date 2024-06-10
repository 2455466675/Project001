using Game.Cfg;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections;
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
        public LoopListView loopListView;

        public TextView textView;

        TeamSystem teamSystem;
        

        private void Start()
        {
            teamSystem = new TeamSystem();
            loopListView.SetDatum(teamSystem.roles[0].items);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("KeyCode.R");
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("KeyCode.R");
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("KeyCode.T");
                List<Role> roleList = teamSystem.roles;
                for (int i = 0; i < roleList.Count; i++)
                {
                    roleList[i].hp.Value = 111;
                }
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log("KeyCode.T");
                List<Role> roleList = teamSystem.roles;
                for (int i = 0; i < roleList.Count; i++)
                {
                    roleList[i].name.Value = "vsews";
                }
            }
        }

        public void OnTest(UINotification notification)
        {
            Debug.Log("OnTest");
            CommandInvoker.ExecuteCommand(new SelectListViewCmd(loopListView));
        }

        public void OnSubmit(UINotification notification) 
        {
            string text = notification.guidable.CurrentGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            Debug.Log($"OnSubmit:{text}");
        }

        public void OnSelect(UINotification notification)
        {
            string text = notification.guidable.CurrentGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            Debug.Log($"OnSelect:{text}");
        }

        public void OnMoveLeft(UINotification notification)
        {
            string text = notification.guidable.CurrentGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            Debug.Log($"OnMoveLeft:{text}");
        }

        public void OnMoveRight(UINotification notification)
        {
            string text = notification.guidable.CurrentGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            Debug.Log($"OnMoveRight:{text}");
        }    
    }

    public class TeamSystem
    {
        public List<Role> roles;

        public TeamSystem() 
        {
            List<RoleCfg> r = GameCore.GameCfgData.FindAll<RoleCfg>();
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
                    items = new ListDB()
                };

                for (int j = 0; j < 20; j++)
                {
                    Item item = new Item(j);
                    role.items.Add(item);
                }

                roles.Add(role);
            }
        }
    }

    public class Item : ItemDB
    {
        public int id;
        public Item(int id)
        {
            this.id = id;
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

        public ListDB items;
        public Role()
        {
            hp = new IntDB(2);
            name = new StringDB("sgew");
            items = new ListDB();
        }

        public void Log()
        {            
            Debug.Log(hp + 2);
            string s = name;
            Debug.Log(s);
        }
    }
}

