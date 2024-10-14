using System.Collections;
using Navigation;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class LoginListProxy : ListProxy
    {
        public override ListName Name => ListName.Login;

        public override WindowId WindowId => WindowId.WinLogin;

    }
}

