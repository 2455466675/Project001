
namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class OverviewController : UIController
	{
        protected override void Register()
        {
            base.Register();
            defaultListView.SetDatum(GameCore.System.OverviewSystem.GetMenus());
        }

        public void OnTest(UINotification notification)
        {
            MLog.Log("OnTest");
            GameCore.UI.OpenWin(100004);
        }
    } 
}

