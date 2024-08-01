
namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectListViewCmd : ICommand
    {
        public bool Undoable => true;

        public SelectListViewCmd()
        {
        }

        public bool Execute()
        {
            return true;
        }

        public bool Undo()
        {
            GameCore.UI.UnSelectListView();
            return true;
        }
    }
}

