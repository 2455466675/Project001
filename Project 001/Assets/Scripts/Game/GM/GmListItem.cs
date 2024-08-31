using Game.UI;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class GmListItem : GuidableItem<GmListItemDB>
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

