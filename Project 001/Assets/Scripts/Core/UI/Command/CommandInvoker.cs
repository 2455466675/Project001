using Game.Core;
using System.Collections.Generic;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class CommandInvoker
	{
        public static Stack<ICommand> commands = new Stack<ICommand>();

        public static void ClearCommands()
        {
            commands?.Clear();
        }

        public static void ExecuteCommand(ICommand command)
        {
            command.Execute();
            commands.Push(command);
        }

        public static void UndoCommand()
        {
            if (commands.Count <=0 ) 
            {
                return;
            }
            ICommand command = commands.Peek();
            if (!command.Undoable) 
            {
                MLog.Log($"此操作不可撤销:{command}");
                return;
            }
            command.Undo();
            commands.Pop();
        }
	}
}

