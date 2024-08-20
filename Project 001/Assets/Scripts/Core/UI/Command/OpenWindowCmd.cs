using System.Collections.Generic;

namespace Game.UI
{
    public class OpenWindowCmd : IUICommand
    {
        public bool IsUndoable
        {
            get
            {
                if (window.Id == (int)WindowId.WinLogin)
                {
                    return subCommand != null && subCommand.Count > 1;
                }

                return true;
            }
        }

        public bool IsEmpty => subCommand == null || subCommand.Count == 0;
        private Window window;
        private Stack<IUICommand> subCommand;

        public OpenWindowCmd(Window window)
        {
            this.window = window;
            subCommand = new Stack<IUICommand>();
        }

        public void Push(IUICommand command) 
        {
            if (subCommand.TryPeek(out IUICommand peekCommand))
            {
                peekCommand.OnSink();
            } 
            subCommand.Push(command);
            command.OnPush();
        }

        public void Pop()
        {
            IUICommand command = subCommand.Pop();
            command.OnPop();

            if (subCommand.TryPeek(out IUICommand peekCommand))
            {
                peekCommand.OnRise();
            }
        }

        public void OnPush()
        {
            if (window == null)
            {
                return;
            }
            GameCore.UI.InFocusWindow(window);
            window.Show();
            window.InFocus();
        }

        public void OnPop()
        {
            if (window == null)
            {
                return;
            }
            window.OutFocus();
            window.Hide();
        }

        public void OnRise()
        {
            if (subCommand.TryPeek(out IUICommand cmd))
            {
                cmd.OnRise();
            }
        }

        public void OnSink()
        {
            if(subCommand.TryPeek(out IUICommand cmd))
            {
                cmd.OnSink();
            }
        }
    }
}
