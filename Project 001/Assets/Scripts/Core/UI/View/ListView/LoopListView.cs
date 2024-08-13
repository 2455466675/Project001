
namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class LoopListView : ListView
	{
        private LoopGuidableBox GuidableBox => box != null ? box as LoopGuidableBox : null;

        public override void Awake()
        {
            base.Awake();
            GuidableBox.Init(IndexChangedHandler, SelectChangedHandler);
        }

        public override void UpdateView()
        {
            base.UpdateView();
            GuidableBox.UpdateTotalCount(Count);
        }

        private void IndexChangedHandler(IndexChangedEventArgs args)
        {
            if (args == null) return;

            GuidableItemBase[] listItems = args.Items;
            if (listItems == null || listItems.Length <= 0) return;
            if (Count <= 0) return;

            for (int i = 0; i < listItems.Length; i++)
            {
                int index = args.MinIndex + i;
                listItems[i].Register(items[index], index);
            }
        }

        private void SelectChangedHandler(SelectChangedEventArgs args)
        {
            if (!IsFocus)
            {
                return;
            }
            if (args == null || !args.IsSuccesss)
            {
                return;
            }          
            GameCore.UI.SelectGuidable(args.Item);
        }
    }
}

