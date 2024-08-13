
namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectGuidableGroupCmd : IUICommand
    {
        public bool IsUndoable => true;

        private IGuidableGroup group;

        public SelectGuidableGroupCmd(IGuidableGroup group)
        {
            this.group = group;
        }

        public void OnPush()
        {
            group.InFocus();
        }

        public void OnPop()
        {
            group.Exit();
        }

        public void OnRise()
        {
            group.InFocus();
        }

        public void OnSink()
        {
            group.OutFocus();
        }
    }
}

