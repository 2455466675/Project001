namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public interface ICommand
	{
        bool Undoable {get;}
        bool Execute();
        bool Undo();
	}
}

