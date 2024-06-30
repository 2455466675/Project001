namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class OpenWindowCmd : ICommand
    {
        public bool Undoable => undoable;
        private readonly int windowId;
        private readonly bool undoable;
        public OpenWindowCmd(int windowId, bool undoable)
        {
            this.windowId = windowId;
            this.undoable = undoable;
        }

        public void Execute()
        {
            GameCore.UI.OpenWin(windowId);
        }

        public void Undo()
        {
            GameCore.UI.CloseWin(windowId);
        }

        public override string ToString()
        {
            return windowId.ToString();
        }
    }
}

