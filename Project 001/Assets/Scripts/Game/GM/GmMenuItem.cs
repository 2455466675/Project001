using Game.UI;

namespace Game.System
{
    /// <summary>
    /// 
    /// </summary>
	public class GmMenuItem : GuidableItem<GmMenuItemDB>
	{
        protected override void OnDatumChange()
        {
            base.OnDatumChange();
          
            TextView textView = GetView<TextView>("buttonName");
            if (textView == null)
            {
                return;
            }

            textView.SetDatum(Dautm.name);
        }
    }
}

