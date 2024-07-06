using Game.Cfg;
using Game.Core;
using OfficeOpenXml.ConditionalFormatting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    [Serializable]
    public class WinGroupItem 
    {
        public UIGroupEnum group;
        public Transform winGroup;
    }

    /// <summary>
    /// 
    /// </summary>
    public class UIRoot : MonoBehaviour
    {
        public Camera UICamera;
        public WinGroupItem[] groups;

        public IGuidableGroup GuidableGroup {get; private set;}
        public IGuidable LastSelectUI {get; private set;}
        public IGuidable CurrSelectUI {get; private set;}
        
        private Dictionary<UIGroupEnum, Transform> groupsMap;
        private event UnityAction<IGuidable> SelectUIEventHandler;
        private event UnityAction<IGuidable> DeselectUIEventHandler;

        private Dictionary<int, Window> windows;
        private Stack<Window> winStack;
        private Stack<ICommand> commands;

        private Stack<IGuidableGroup> guidableGroups;

        public void Awake()
        {
            DontDestroyOnLoad(this);
            UICamera = GetComponentInChildren<Camera>();

            InitWinGroup();

            windows = new Dictionary<int, Window>();
            winStack = new Stack<Window>();
            commands = new Stack<ICommand>();
            guidableGroups = new Stack<IGuidableGroup>();
        }

        private void InitWinGroup()
        {
            if (groups != null && groups.Length > 0)
            {
                groupsMap = new Dictionary<UIGroupEnum, Transform>();
                for (int i = 0; i < groups.Length; i++)
                {
                    groupsMap.Add(groups[i].group, groups[i].winGroup);
                }
            }
        }

        public void AddSelectUIEventListener(UnityAction<IGuidable> action)
        {
            SelectUIEventHandler += action;
        }

        public void AddDeselectUIEventListener(UnityAction<IGuidable> action)
        {
            DeselectUIEventHandler += action;
        }

        public void RemoveSelectUIEventListener(UnityAction<IGuidable> action)
        {
            SelectUIEventHandler -= action;
        }

        public void RemoveDeselectUIEventListener(UnityAction<IGuidable> action)
        {
            DeselectUIEventHandler -= action;
        }

        public void Submit()
        {
            CurrSelectUI?.OnSubmit();
        }

        public void SelectUI(IGuidable guidableItem)
        {
            DeselectUI(CurrSelectUI);
            CurrSelectUI = guidableItem;
            CurrSelectUI?.OnSelect();
            SelectUIEventHandler?.Invoke(CurrSelectUI);
        }

        public void DeselectUI(IGuidable guidableItem)
        {
            LastSelectUI = guidableItem;
            LastSelectUI?.OnDeselect();
            DeselectUIEventHandler?.Invoke(LastSelectUI);
        }

        public void SelectListView(IGuidableGroup guidableGroup)
        {

            if (GuidableGroup == null)
            {
                GuidableGroup = guidableGroup;
                GuidableGroup.Layer = 1;
                GuidableGroup.InFocus();
                guidableGroups.Push(GuidableGroup);
                return;
            }
            
            if (!guidableGroups.Contains(guidableGroup))
            {
                guidableGroup.Layer = GuidableGroup.Layer + 1;
                guidableGroups.Push(guidableGroup);
            } 

            if (guidableGroup.Layer > GuidableGroup.Layer)
            {
                GuidableGroup.OutFocus();
                ExecuteCommand(new SelectListViewCmd(GuidableGroup));
            }
            else
            {
                GuidableGroup.Exit();
            }
     
            GuidableGroup = guidableGroup;
            GuidableGroup.InFocus();
            MLog.Log("SelectListView", (guidableGroup as ListView).name, guidableGroups.Count);
        }

        public void OpenWin(int id)
        {
            Window inst = GetWindowInst(id);
            if (inst == null) 
            {
                return;
            }

            WindType windType = inst.winType;
            if (windType == WindType.Base)
            {
                ExecuteCommand(new OpenBaseWinCmd(id));
            }
            else if(windType == WindType.Guide)
            {
                
            }
            else
            {
                ExecuteCommand(new OpenNormalWinCmd(id));
            }
            //inst.transform.SetAsLastSibling();
            inst.Show();
            inst.Enter();
            winStack.Push(inst);
        }

        public void CloseWin(int id)
        {
            if (winStack == null || winStack.Count <= 0)
            {
                return;
            }

            Window win = winStack.Pop();
            if (win.Id != id)
            {
                MLog.Warn($"要关闭的窗口不是顶层窗口：{id}");
                return;
            }
            win.Exit();
            win.Hide();

            if (winStack.Count <= 0)
            {
                return;
            }

            if (winStack.TryPeek(out Window topWin))
            {
                topWin.Enter();
            }           
        }

        /// <summary>
        /// 关闭所有界面
        /// </summary>
        public void CloseAllWin()
        {
            MLog.Log("CloseAllWin");
            foreach (var item in winStack)
            {
                item.Exit();
                item.Hide();
            }
            winStack?.Clear();
            commands?.Clear();
            guidableGroups?.Clear();
            GuidableGroup = null;
            LastSelectUI = null;
            CurrSelectUI = null;
        }

        /// <summary>
        /// 执行一个命令
        /// </summary>
        /// <param name="command"></param>
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            commands.Push(command);
        }

        /// <summary>
        /// 撤销一个命令
        /// </summary>
        public void UndoCommand()
        {
            if (commands.Count <= 0)
            {
                return;
            }
            ICommand command = commands.Peek();
            bool succ = command.Undo();
            if (!succ)
            {
                MLog.Log($"此操作不可撤销");
                return;
            }
            if (commands.Count > 0)
            {
                commands.Pop();
            }
        }

        private Window GetWindowInst(int id)
        {
            if (windows.ContainsKey(id))
            {
                return windows[id];
            }

            WindowCfg cfg = GameCore.GameCfgData.FindById<WindowCfg>(id);
            if (cfg == null)
            {
                MLog.Error($"没有窗体配置:{id}");
                return null;
            }
            GameObject prefab = GameCore.ResourceManager.LoadAsset<GameObject>(cfg.path);
            if (prefab == null)
            {
                MLog.Error($"没有窗体资源:{cfg.path}");
                return null;
            }
            Window win = prefab.GetComponent<Window>();
            Window inst = Instantiate(win, groupsMap[win.group], false);
            inst.SetCfg(cfg);

            return inst;
        }
    }
}