using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class NavigationPanelCommand : INavigationCommand
    {
        public bool IsUndoable => true;

        public WindowId Id => (WindowId)window.Id;

        private Window window;

        private Stack<NavigationListCommand> commands;

        public NavigationPanelCommand(Window window)
        {
            this.window = window;
            commands = new Stack<NavigationListCommand>();
        }

        public NavigationListCommand Peek()
        {
            return commands.Peek();
        }

        public bool Pop()
        {
            if (commands.Count <= 0)
            {
                return true;
            }
            NavigationListCommand cmd = commands.Pop();
            cmd.OnPop();
            if (commands.TryPeek(out NavigationListCommand popCmd))
            {
                popCmd.OnRise();                
            }
            return commands.Count <= 0;
        }

        public bool Push(NavigationListCommand cmd)
        {
            if (cmd.OnPush())
            {
                if (commands.TryPeek(out NavigationListCommand popCmd))
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
            GameCore.UI.HideWindow();
        }

        public bool OnPush()
        {
            return true;
        }

        public bool OnRise()
        {
            if (commands.TryPeek(out NavigationListCommand popCmd))
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
            if (commands.TryPeek(out NavigationListCommand popCmd))
            {
                return popCmd.OnSink();
            }
            else
            {
                return false;
            }            
        }
    }
}

