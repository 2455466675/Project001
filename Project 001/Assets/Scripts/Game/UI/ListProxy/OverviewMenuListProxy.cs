using System.Collections;
using Navigation;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewMenuListProxy : ListProxy
    {
        public override ListName Name => ListName.OverviewMenu;

        public override WindowId WindowId => WindowId.WinOverview;

    }
}

