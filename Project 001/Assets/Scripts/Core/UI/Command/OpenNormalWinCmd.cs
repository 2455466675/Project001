namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class OpenNormalWinCmd : ICommand
    {
        public bool Undoable => true;

        private readonly int id;
        public OpenNormalWinCmd(int id)
        {
            this.id = id;         
        }

        public bool Execute()
        {
            return true;
        }

        public bool Undo()
        {
            GameCore.UI.CloseWin(id);
            return true;
        }
    }
}

