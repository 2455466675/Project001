namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UINotification
    {
        public ListItem ListItem {get; private set;}

        public UINotification(ListItem listItem)
        {
            this.ListItem = listItem;
        }
    }
}