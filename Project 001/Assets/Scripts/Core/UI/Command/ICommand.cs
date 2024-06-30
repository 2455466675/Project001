namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public interface ICommand
	{
        bool Undoable {get;}
        void Execute();
        void Undo();
	}

    public class SelectListViewCmd : ICommand
    {
        public bool Undoable => true;

        private readonly IGuidableGroup group1;
        private readonly IGuidableGroup group2;
        public SelectListViewCmd(IGuidableGroup group)
        {
            group1 = GameCore.UI.UIRoot.GuidableGroup;
            group2 = group;
            group2.Layer = group1.Layer + 1;
        }

        public void Execute()
        {
            GameCore.UI.SelectGuidableGroup(group2);
        }

        public void Undo()
        {            
            GameCore.UI.SelectGuidableGroup(group1);
        }
    }
}

