using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
	public class CommandInvoker
	{
        public static Stack<ICommand> commands = new Stack<ICommand>();

        public static void ExecuteCommand(ICommand command)
        {
            command.Execute();
            commands.Push(command);
            Debug.Log($"Execute:{commands.Count}");
        }

        public static void UndoCommand()
        {
            if (commands.Count <=0 ) 
            {
                return;
            }
            ICommand command = commands.Pop();
            command.Undo();
            Debug.Log($"Undo:{commands.Count}");
        }
	}
}

