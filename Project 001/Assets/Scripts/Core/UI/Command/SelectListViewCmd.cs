
namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectListViewCmd : ICommand
    {
        public bool Undoable => true;

        private readonly IGuidableGroup group;

        public SelectListViewCmd(IGuidableGroup group)
        {
            this.group = group;
        }

        public bool Execute()
        {
            return true;
        }

        public bool Undo()
        {
            GameCore.UI.SelectListView(group);
            return true;
        }
    }
}

