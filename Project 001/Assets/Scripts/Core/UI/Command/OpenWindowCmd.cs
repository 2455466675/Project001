using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class OpenWindowCmd : ICommand
    {
        private readonly int windowId;
        public OpenWindowCmd(int windowId)
        {
            this.windowId = windowId;
        }

        public void Execute()
        {
            GameCore.UI.OpenWin(windowId);
        }

        public void Undo()
        {
            GameCore.UI.CloseWin(windowId);
        }
    }
}

