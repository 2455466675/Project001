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
    }
}

