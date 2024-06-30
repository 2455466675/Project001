using Game.Core;

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class LoopListView : ListView
	{
        public LoopNavigationBox LoopBox 
        {
            get 
            {
                if (box == null)
                {
                    box = GetComponent<LoopNavigationBox>();
                }
                if (box == null)
                {
                    return null;
                }
                return box as LoopNavigationBox;
            }
        }

        public override void Awake()
        {
            base.Awake();
            InitBox();
        }

        public override void UpdateView()
        {
            base.UpdateView();
            if (LoopBox == null) return;
            MLog.Log($"UpdateView:{Count}");
            LoopBox.UpdateTotalCount(Count);
        }

        private void InitBox()
        {
            if (LoopBox == null) return;
            LoopBox.Init(OnBoxChange);
        }

        private void OnBoxChange(int min, int max, ListItem[] listItems)
        {
            MLog.Log($"Count:{Count}");
            if (listItems == null || listItems.Length <= 0) return;
            if (Count <= 0) return;
            
            for (int i = 0; i < listItems.Length; i++)
            {
                listItems[i].Register(items[min + i], min + i);
            }
        }
    }
}

