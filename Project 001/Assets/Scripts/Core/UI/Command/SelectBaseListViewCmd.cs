using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
	public class SelectBaseListViewCmd : ICommand
    {
        public bool Undoable => true;

        private readonly int windowId;

        public SelectBaseListViewCmd(int windowId)
        {
            this.windowId = windowId;
        }

        public bool Execute()
        {
            return true;
        }

        public bool Undo()
        {
            GameCore.UI.UnSelectListView();
            GameCore.UI.CloseWin(windowId);
            return true;
        }
    }
}

