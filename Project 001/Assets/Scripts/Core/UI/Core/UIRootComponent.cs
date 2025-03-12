using EC;
using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UIRootComponent : EC.Component, IInitializable
    {
        private UIRoot uiRoot;

        private NavigationConfig navigationConfig;

        private List<PanelComponent> panels;
        private List<NavigationGroupComponent> groups;

        private Stack<PanelCommand> commands;

        public IEnumerator Init(GameInitCfg intCfg)
        {
            ResourceComponent rc = World.GetComponent<ResourceComponent>();

            GameObject uiRootGo = rc.LoadAndInstantiate(intCfg.UIRootPath, null);
            uiRoot = uiRootGo.GetComponent<UIRoot>();

            navigationConfig = rc.LoadAsset<NavigationConfig>(intCfg.NavigationConfigFilePath);
            navigationConfig.Init();

            panels = new List<PanelComponent>();
            groups = new List<NavigationGroupComponent>();
            commands = new Stack<PanelCommand>();

            yield return uiRoot;
        }

        public PanelComponent ShowPanel(UIDefine.Panel_ID panelID)
        {
            PanelComponent pc = panels.Find(p => p.PanelID == panelID);
            if (pc != null)
            {
                pc.Show();
            }
            else
            {
                Entity panelEntity = Entity.CreateChild();
                pc = panelEntity.AddComponent<PanelComponent>();
                panels.Add(pc);

                pc.Init(panelID);
                pc.Show();
            }

            return pc;
        }

        public void HidePanel(UIDefine.Panel_ID panelID)
        {
            PanelComponent pc = panels.Find(p => p.PanelID == panelID);
            pc?.Hide();
        }

        public void AddNavigationGroup(NavigationGroup navigationGroup) 
        {
            if (navigationGroup == null)
            {
                return;
            }

            var group = groups.Find(g => g.GroupID == navigationGroup.groupID);
            if (group != null)
            {
                return;
            }

            Entity entity; 
            UIDefine.Panel_ID panelID = navigationConfig.GetMap(navigationGroup.groupID);
            PanelComponent pc = panels.Find(p => p.PanelID == panelID);
            if(pc == null) 
            {
                ShowPanel(panelID);
                pc = panels.Find(p => p.PanelID == panelID);
                if (pc == null) 
                {
                    return;
                }
            }

            entity = pc.Entity.CreateChild();
            group = entity.AddComponent<NavigationGroupComponent>();
            group.Init(navigationGroup);
            groups.Add(group);
        }

        public void RemoveNavigationGroup(UIDefine.Group_ID groupID) 
        {
            var group = groups.Find(g => g.GroupID == groupID);
            if (group == null)
            {
                return;
            }
            groups.Remove(group);
            World.DestroyEntity(group.Entity);
        }

        public NavigationGroupComponent GetNavigationGroup(UIDefine.Group_ID groupID) 
        {
            if (groupID == UIDefine.Group_ID.Undefined) 
            {
                return null;
            }

            var group = groups.Find(g => g.GroupID == groupID);
            return group;           
        }

        public void Navigate(UIDefine.Group_ID groupID) 
        {
            World.GetComponent<InputComponent>().PushInputMode(InputMode.UI);

            UIDefine.Panel_ID panelID = navigationConfig.GetMap(groupID);

            ShowPanel(panelID);

            GroupCommand groupCmd = new(groupID, new int[] {0});
            //依附的界面已经打开了
            if (commands.TryPeek(out PanelCommand popCmd) && popCmd.PanelID == panelID)
            {
                //直接加入
                if (popCmd.Push(groupCmd) && popCmd.OnPush())
                {

                }
                else
                {
                    //Back();
                }
            }
            else
            {
                //新建一个界面命令
                PanelCommand panelCmd = new(panelID);
                if (panelCmd.Push(groupCmd) && panelCmd.OnPush())
                {
                    popCmd?.OnSink();
                    commands.Push(panelCmd);
                }
                else
                {
                    panelCmd.Pop();
                    panelCmd.OnPop();
                    return;
                }
            }
        }

        public void Move(Vector2 dir) 
        {
            if (commands.TryPeek(out PanelCommand cmd))
            {
                var group = GetNavigationGroup(cmd.Peek().GroupID);
                group?.Move(dir);
            }
        }

        /// <summary>
        /// 确定（Enter键、空格、鼠标左键）
        /// </summary>
        public void Submit() 
        {
            if (commands.TryPeek(out PanelCommand cmd))
            {
                var group = GetNavigationGroup(cmd.Peek().GroupID);
                group?.Submit();
            }
        }

        /// <summary>
        /// 后退（C键、鼠标右键）
        /// </summary>
        /// <returns></returns>
        public bool Back()
        {
            if (commands.TryPeek(out PanelCommand cmd))
            {
                if (!cmd.IsUndoable)
                {
                    MLog.Log("not Undoable panel:", cmd.PanelID);
                    return false;
                }

                if (cmd.Pop())
                {
                    cmd.OnPop();
                    commands.Pop();
                    if (commands.TryPeek(out PanelCommand cmd2))
                    {
                        cmd2.OnRise();
                    }
                }
            }

            if (commands.Count <= 0)
            {
                World.GetComponent<InputComponent>().PopInputMode(InputMode.UI);
            }
            return true;
        }

        /// <summary>
        /// 关闭（ESC键）
        /// </summary>
        public void Close(bool compulsory)
        {
            if (compulsory) 
            {
                while (commands.Count > 0)
                {
                    if (commands.TryPop(out PanelCommand cmd))
                    {
                        var subCmds = cmd.Commands;
                        while (subCmds.Count > 0)
                        {
                            if (subCmds.TryPop(out GroupCommand subCmd)) 
                            {
                                subCmd.OnPop();                            
                            }
                            else
                            {
                                break;
                            }
                        }
                        cmd.OnPop();
                    }
                    else
                    {
                        break;
                    }
                }      
                commands.Clear();
            }
            else
            {                
                while (commands.Count > 0)
                {
                    if (!Back()) 
                    {
                        break;
                    }
                }
            }
            World.GetComponent<InputComponent>().PopInputMode(InputMode.UI);
        }

        public WindowGroup GetWinGroup(UIGroup group) 
        { 
            return uiRoot.GetWinGroup(group);
        }
    }
}
