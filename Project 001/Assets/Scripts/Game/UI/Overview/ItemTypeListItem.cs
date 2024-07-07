namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class ItemTypeListItem : ListItem
	{
        public TextView textView;

        protected override void OnDatumChange()
        {
            base.OnDatumChange();
            ItemTypeItemDB db = GetListItem<ItemTypeItemDB>();
            if (db != null)
            {
                if (textView != null)
                {
                    MLog.Log("ItemTypeListItem", db.name.StringValue);
                    textView.SetDatum(db.name);
                }
            }
        }
    }
}

