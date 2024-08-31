
namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewController : UIController
	{
        protected override void Register()
        {
            ListView.Register(ListViewId.OverviewMenuList, GameCore.System.OverviewSystem.GetMenus());
        }

        public void OnTest(UINotification notification)
        {
            MLog.Log("OnTest");
            GameCore.UI.OpenWindow(WindowId.WinPackage);
        }
    } 
}

