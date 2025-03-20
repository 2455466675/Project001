namespace Game.UI
{
    public class TestNavigationItem : GameNavigationItem
    {
        protected override void OnRefresh()
        {
            TextView view = GetView<TextView>();
            if (view != null) 
            {
                view.SetTextByStr(GetData().ToString());
            }
        }
    }
}