using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class ItemTypeListItem : GuidableItemBase
    {       
        protected override void OnDatumChange()
        {
            base.OnDatumChange();
            ItemTypeItemDB db = GetItemDB<ItemTypeItemDB>();
            if (db == null)
            {
                return;
            }

            TextView textView = GetView<TextView>("buttonName");
            if (textView == null)
            {
                return;
            }

            textView.SetDatum(db.name);
        }
    }
}

