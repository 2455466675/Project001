using Game.System;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class OverviewMenuNavigationItem : NavigationItem
    {
        public TextView menuText;

        protected override void OnRefresh()
        {
            base.OnRefresh();

            OverviewMenuItem item = data as OverviewMenuItem;
            if (item != null) 
            {
                menuText.SetTextById(item.cfg.Name);
            }
        }
    }
}
