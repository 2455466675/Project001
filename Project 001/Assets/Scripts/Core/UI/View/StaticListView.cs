
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class StaticListView : ListView
	{
        protected StaticGuidableBox StaticBox => box != null ? box as StaticGuidableBox : null;
        protected int[] index;

        public override void Awake()
        {
            base.Awake();
            if (StaticBox != null) 
            {
                StaticBox.Init(SelectChangedHandler);
            }
        }

        public override void UpdateView()
        {
            base.UpdateView();

            if (Count <= 0) return;
            
            for (int i = 0; i < items.Count; i++) 
            {
                GuidableItemBase item = StaticBox.GetItem(i);
                if (item != null) 
                {
                    item.Register(items[i], i);
                }
            }
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

