using Game.UI;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class ItemTypeListItem : GuidableItem<ItemTypeItemDB>
    {       
        protected override void OnDatumChange()
        {                      
            TextView textView = GetView<TextView>("buttonName");
            if (textView == null)
            {
                return;
            }

            textView.SetDatum(Dautm.name);
        }
    }
}

