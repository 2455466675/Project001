using Game.System;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class GmCmd : UIController
	{
        protected override void Register()
        {
            base.Register();
            ListView.GetView(ListViewId.GmMenuList).SetDatum(GameCore.System.Gm.menus);
            ListView.GetView(ListViewId.GmItemList).SetDatum(GameCore.System.Gm.items);
        }

        public void OnSelectMenu(UINotification notification)
        {
            GmMenuItemDB db = notification.ListItem.GetItemDB<GmMenuItemDB>();
            GameCore.System.Gm.SelectMenu(db);
        }

        public void OnSubmitMenu(UINotification notification)
        {
            MLog.Log("OnSubmitMenu", notification.ListItem.GetItemDB<GmMenuItemDB>().name.StringValue);
            GameCore.UI.SelectNavigatable(ListViewId.GmItemList);
        }

        public void OnSubmitItem(UINotification notification)
        {       
            GmListItemDB db = notification.ListItem.GetItemDB<GmListItemDB>();
            GameCore.System.Gm.ExecuteCmd(db.cmdId, db.args);
        }
    }
}

