using Game.Core;
using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class PanelCommand : INavigationCommand
    {
        public bool IsUndoable => CheckUndoable();

        public UIDefine.Panel_ID PanelID { get; private set; }

        private Stack<GroupCommand> commands;

        public PanelCommand(UIDefine.Panel_ID panelID)
        {
            PanelID = panelID;
            commands = new Stack<GroupCommand>();
        }

        public GroupCommand Peek()
        {
            return commands.Peek();
        }

        /// <summary>
        /// 弹出最上面的一个列表命令
        /// </summary>
        /// <returns>执行此操作后是否已空</returns>
        public bool Pop()
        {
            if (commands.Count <= 0)
            {
                return true;
            }
            GroupCommand cmd = commands.Pop();
            cmd.OnPop();
            if (commands.TryPeek(out GroupCommand popCmd))
            {
                popCmd.OnRise();
            }
            return commands.Count <= 0;
        }

        /// <summary>
        /// 压入一个列表命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool Push(GroupCommand cmd)
        {
            if (cmd.OnPush())
            {
                if (commands.TryPeek(out GroupCommand popCmd))
                {
                    popCmd.OnSink();
                }
                commands.Push(cmd);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnPop()
        {
            commands.Clear();
            GameWorld.Instance.GetComponent<UIComponent>().HidePanel(PanelID);
        }

        public bool OnPush()
        {
            return true;
        }

        public bool OnRise()
        {
            if (commands.TryPeek(out GroupCommand popCmd))
            {
                return popCmd.OnRise();
            }
            else
            {
                return false;
            }
        }

        public bool OnSink()
        {
            if (commands.TryPeek(out GroupCommand popCmd))
            {
                return popCmd.OnSink();
            }
            else
            {
                return false;
            }
        }

        private bool CheckUndoable()
        {
            if (commands.TryPeek(out GroupCommand popCmd))
            {
                return popCmd.IsUndoable;
            }
            else
            {
                MLog.Error("CheckUndoable is error");
                return false;
            }
        }
    }
}
