namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class PackageItem : NavigationItem
    {
        public TextView textView;

        protected override void OnRefresh()
        {
            textView.SetTextByStr(data.ToString());
        }
    }
}
