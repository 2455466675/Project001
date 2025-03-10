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

        public Stack<GroupCommand> Commands { get; private set; }

        public PanelCommand(UIDefine.Panel_ID panelID)
        {
            PanelID = panelID;
            Commands = new Stack<GroupCommand>();
        }

        public GroupCommand Peek()
        {
            return Commands.Peek();
        }

        /// <summary>
        /// 弹出最上面的一个列表命令
        /// </summary>
        /// <returns>执行此操作后是否已空</returns>
        public bool Pop()
        {
            if (Commands.Count <= 0)
            {
                return true;
            }
            GroupCommand cmd = Commands.Pop();
            cmd.OnPop();
            if (Commands.TryPeek(out GroupCommand popCmd))
            {
                popCmd.OnRise();
            }
            return Commands.Count <= 0;
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
                if (Commands.TryPeek(out GroupCommand popCmd))
                {
                    popCmd.OnSink();
                }
                Commands.Push(cmd);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnPop()
        {
            Commands.Clear();
            GameWorld.Instance.GetComponent<UIComponent>().HidePanel(PanelID);
        }

        public bool OnPush()
        {
            return true;
        }

        public bool OnRise()
        {
            if (Commands.TryPeek(out GroupCommand popCmd))
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
            if (Commands.TryPeek(out GroupCommand popCmd))
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
            if (Commands.TryPeek(out GroupCommand popCmd))
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
