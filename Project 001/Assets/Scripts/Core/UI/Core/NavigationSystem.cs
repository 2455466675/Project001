using Game.Core;
using System.Collections.Generic;
using UnityEngine;
using Navigation;
using Cysharp.Threading.Tasks;
using Game.System;
using System;
using static UnityEditor.Progress;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class NavigationSystem
	{     
        public GuidableItem[] Current => current;
        private GuidableItem[] current;
        private NavigationList currentList;

        private Dictionary<ListName, ListProxy> proxys;
        private Stack<NavigationPanelCommand> commands;
        private Stack<NavigationListCommand> listCmds;

        public NavigationSystem()
        {
            commands = new Stack<NavigationPanelCommand>();
            listCmds = new Stack<NavigationListCommand>();

            proxys = new Dictionary<ListName, ListProxy>();
            proxys[ListName.Login] = new LoginListProxy();
            proxys[ListName.OverviewMenu] = new OverviewMenuListProxy();
            proxys[ListName.PackageMenu] = new PackageMenuListProxy();
            proxys[ListName.PackageList] = new PackageListListProxy();
            proxys[ListName.GmMenuList] = new GmMenuListProxy();
            proxys[ListName.GmItemList] = new GmItemListProxy();
            proxys[ListName.BattlePlayerList] = new BattlePlayerListProxy();
            proxys[ListName.BattleEnemyList] = new BattleEnemyListProxy();
            proxys[ListName.BattleActionList] = new BattleActionListProxy();
            proxys[ListName.BattleActionList2] = new BattleActionList2Proxy();
        }

        public void Enter(ListName listName, Action<GuidableItem[]> submitAction, int[] indexs)
        {
            if (proxys.TryGetValue(listName, out ListProxy proxy))
            {
                EnterInner(proxy, submitAction, indexs);
            }
            else
            {
                MLog.Error("未注册列表代理");
            }
        }

        /// <summary>
        /// 程序内部强制退出
        /// </summary>
        public void Exit()
        {
            while (listCmds.Count > 0)
            {
                NavigationListCommand cmd = listCmds.Pop();
                cmd.OnPop();
            }

            while (commands.Count > 0)
            {
                NavigationPanelCommand cmd = commands.Pop();
                cmd.OnPop();
            }

            Deselect();
            ExitInner();
        }

        /// <summary>
        /// 后退键
        /// </summary>
        /// <returns></returns>
        public bool Back()
        {
            if (commands.TryPeek(out NavigationPanelCommand cmd))
            {
                if (!cmd.IsUndoable)
                {
                    return false;
                }

                if (cmd.Pop())
                {
                    cmd.OnPop();
                    commands.Pop();
                    if (commands.TryPeek(out NavigationPanelCommand cmd2))
                    {
                        cmd2.OnRise();
                    }
                }
            }

            if (commands.Count <= 0)
            {
                ExitInner();
            }
            return true;
        }

        /// <summary>
        /// ESC键
        /// </summary>
        public void Close()
        {
            while (commands.Count > 0 && Back())
            {

            }
        }

        public void Move(Vector2 dir)
        {
            if (commands.TryPeek(out NavigationPanelCommand cmd))
            {
                cmd.Peek().Proxy.Move(dir);
            }            
        }

        public void Submit()
        {
            if (commands.TryPeek(out NavigationPanelCommand cmd))
            {
                cmd.Peek().Proxy.OnSubmit();
            }

            if (current != null)
            {
                foreach (var item in current)
                {
                    item.OnSubmit();
                }
            }
        }

        public void Select(NavigationList list, params GuidableItem[] items)
        {
            Deselect();

            if (list == null || items == null || items.Length == 0)
            {
                return;
            }
            
            currentList = list;
            current = items;

            if (current != null)
            {
                foreach (var item in current)
                {
                    item.OnSelect();
                }
            }
        }

        private async void EnterInner(ListProxy proxy, Action<GuidableItem[]> submitAction, int[] indexs)
        {            
            await proxy.Precondition();

            if (!proxy.IsValid)
            {
                MLog.Error($"列表资源加载错误{proxy.Name}, {proxy.WindowId}");
                return;
            }
            
            while (!proxy.IsReady)
            {
                await UniTask.DelayFrame(1);
            }

            proxy.Register(submitAction);

            NavigationListCommand listCmd = new NavigationListCommand(proxy, indexs);
            //依附的界面已经打开了
            if (commands.TryPeek(out NavigationPanelCommand popCmd) && popCmd.Id == proxy.WindowId)
            {
                //直接加入
                if (popCmd.Push(listCmd) && popCmd.OnPush())
                {

                }
                else
                {
                    MLog.Error($"此列表没有有效目标{proxy.Name}, {proxy.WindowId}");
                    Back();
                }
            }
            else
            {
                //新建一个界面命令
                NavigationPanelCommand panelCmd = new NavigationPanelCommand(proxy.Parent);
                if (panelCmd.Push(listCmd) && panelCmd.OnPush())
                {
                    popCmd?.OnSink();                    
                    commands.Push(panelCmd);
                }
                else
                {
                    MLog.Error($"此列表没有有效目标{proxy.Name}, {proxy.WindowId}");
                    panelCmd.Pop();
                    panelCmd.OnPop();
                    return;
                }
            }

            listCmds.Push(listCmd);
            GameCore.StateController.SwitchModel(GameMode.UI);
        }

        private void ExitInner()
        {
            listCmds.Clear();
            commands.Clear();
            GameCore.StateController.SwitchModel(GameMode.SCENE);
        }

        private void Deselect()
        {
            if (current != null)
            {                
                for (int i = 0; i < current.Length; i++)
                {
                    current[i].OnDeselect();
                }
            }
        }
    }
}

