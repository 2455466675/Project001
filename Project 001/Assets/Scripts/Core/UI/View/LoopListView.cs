
namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class LoopListView : ListView
	{
        private LoopGuidableBox GuidableBox => box != null ? box as LoopGuidableBox : null;

        private int[] index;

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

        public override void InFocus()
        {
            base.InFocus();
            index ??= new int[] { 0 };
            box.Select(index);
        }

        public override void OutFocus()
        {
            base.OutFocus();
            index = box.CurrIndex;
        }

        public override void Exit()
        {
            base.Exit();
            index = null;
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
            GameCore.UI.SelectGuidable(args.Items);
        }
    }
}

