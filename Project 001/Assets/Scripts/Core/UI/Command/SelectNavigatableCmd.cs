
namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectNavigatableCmd : IUICommand
    {
        public bool IsUndoable => true;

        private INavigatable navigatable;

        public SelectNavigatableCmd(INavigatable navigatable)
        {
            this.navigatable = navigatable;
        }

        public void OnPush()
        {
            GameCore.UI.InFocusGroup(navigatable);
            navigatable.InFocus();
        }

        public void OnPop()
        {
            navigatable.Exit();
        }

        public void OnRise()
        {
            GameCore.UI.InFocusGroup(navigatable);
            navigatable.InFocus();
        }

        public void OnSink()
        {
            navigatable.OutFocus();
        }
    }
}

