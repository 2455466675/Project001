namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class ItemTypeListItem : ListItem
	{       
        protected override void OnDatumChange()
        {
            base.OnDatumChange();
            ItemTypeItemDB db = GetListItem<ItemTypeItemDB>();
            if (db == null)
            {
                return;
            }

            TextView textView = GetView<TextView>("buttonName");
            if (textView == null)
            {
                return;
            }
            MLog.Log("ItemTypeListItem", db.name.StringValue);
            textView.SetDatum(db.name);
        }
    }
}

