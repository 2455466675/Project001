using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;
using Cysharp.Threading.Tasks;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class NavigationSystem
	{     
        private GuidableItem[] current;
        public GuidableItem[] Current => current;

        private Dictionary<ListName, ListProxy> proxys;

        private Stack<NavigationPanelCommand> commands;

        public NavigationSystem()
        {
            commands = new Stack<NavigationPanelCommand>();

            proxys = new Dictionary<ListName, ListProxy>();
            proxys[ListName.Login] = new LoginListProxy();
            proxys[ListName.OverviewMenu] = new OverviewMenuListProxy();
            proxys[ListName.PackageMenu] = new PackageMenuListProxy();
            proxys[ListName.PackageList] = new PackageListListProxy();
        }

        public void Enter(ListName listName)
        {
            if (proxys.TryGetValue(listName, out ListProxy proxy))
            {
                EnterInner(proxy);
            }
        }

        public void Exit()
        {
            commands.Clear();
            GameCore.StateController.SwitchModel(GameMode.SCENE);
        }

        public void Back()
        {
            if (commands.TryPeek(out NavigationPanelCommand cmd))
            {
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
                Exit();
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
            if (current != null)
            {
                foreach (var item in current)
                {
                    MLog.Log("OnSubmit", item.gameObject.name);
                    item.OnSubmit();
                }
            }
        }

        public void Select(params GuidableItem[] items)
        {
            if (current != null)
            {
                foreach (var item in current)
                {
                    item.OnDeselect();
                }
            }

            if (items == null || items.Length == 0)
            {
                return;
            }
            
            current = items;

            if (current != null)
            {
                foreach (var item in current)
                {
                    MLog.Log("OnSelect", item.gameObject.name);
                    item.OnSelect();
                }
            }
        }

        private async void EnterInner(ListProxy proxy)
        {
            if (proxy.WindowId == WindowId.WinLogin)
            {
                proxy.LoadWindow();
            }
            else
            {
                await proxy.LoadWindowAsync();
            }

            if (!proxy.IsValid)
            {
                MLog.Error($"列表资源加载错误{proxy.Name}, {proxy.WindowId}");
                return;
            }
            
            NavigationListCommand listCmd = new NavigationListCommand(proxy);

            if (commands.TryPeek(out NavigationPanelCommand popCmd) && popCmd.Id == proxy.WindowId)
            {
                popCmd.Push(listCmd);
            }
            else
            {
                NavigationPanelCommand panelCmd = new NavigationPanelCommand(proxy.Parent);
                if (panelCmd.Push(listCmd) && panelCmd.OnPush())
                {
                    popCmd?.OnSink();                    
                    commands.Push(panelCmd);
                }
                else
                {
                    MLog.Error($"此列表没有有效目标{proxy.Name}, {proxy.WindowId}");
                    return;
                }
            }

            GameCore.StateController.SwitchModel(GameMode.UI);
        }
    }
}

