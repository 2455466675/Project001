using Game;
using MVC;
using UnityEngine;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
	public class FixedListView : ListView
    {
        [SerializeField]
        private FixedNavigationList list;

        public void Awake()
        {
            if (list != null)
            {
                list.Init(SelectChangedHandler);
            }
        }

        protected override void Start()
        {
            base.Start();
            list.IsValid = IsRegistered;
        }

        protected override void OnUpdateView()
        {
            base.OnUpdateView();
            if (list == null)
            {
                return;
            }

            for (int i = 0; i < Count; i++)
            {
                GuidableItem item = list.GetItem(i);
                if (item != null)
                {
                    item.SetDatum(Datas[i]);
                }
            }
            list.UpdateTotalCount(Count);
        }

        private void SelectChangedHandler(SelectChangedEventArgs args)
        {
            if (args == null || !args.IsSuccesss)
            {
                return;
            }
            GameCore.UI.Select(args.Items);
        }
    }
}

