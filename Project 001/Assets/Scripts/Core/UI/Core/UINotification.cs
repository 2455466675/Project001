namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class UINotification
    {
        public GuidableItemBase ListItem {get; private set;}

        public UINotification(GuidableItemBase listItem)
        {
            this.ListItem = listItem;
        }
    }
}