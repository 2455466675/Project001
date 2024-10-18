using Cysharp.Threading.Tasks;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class LoginListProxy : ListProxy
    {
        public override ListName Name => ListName.Login;

        public override WindowId WindowId => WindowId.WinLogin;

        public override bool IsUndoable()
        {
            return false;
        }

        public override UniTask Precondition()
        {
            Window window = GameCore.UI.ShowWindow(WindowId);
            parent = window;
            list = window.Find(Name);
            return UniTask.DelayFrame(1);
        }
    }
}

