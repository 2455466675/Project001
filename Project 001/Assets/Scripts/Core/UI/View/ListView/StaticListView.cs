
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class StaticListView : ListView
	{
        private StaticGuidableBox StaticBox => box != null ? box as StaticGuidableBox : null;

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

