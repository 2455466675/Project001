using UnityEngine;
using MVC;
using Game;

namespace Navigation
{
    /// <summary>
    /// 
    /// </summary>
	public class LoopListView : ListView
    {
        [SerializeField]
        private LoopNavigationList list;

        public void Awake()
        {
            if (list != null)
            {
                list.Init(IndexChangedHandler, SelectChangedHandler);
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
            list.UpdateTotalCount(Count);
        }

        private void IndexChangedHandler(IndexChangedEventArgs args)
        {
            if (args == null) return;

            GuidableItem[] listItems = args.Items;
            if (listItems == null || listItems.Length <= 0) return;
            if (Count <= 0) return;

            for (int i = 0; i < listItems.Length; i++)
            {
                int index = args.MinIndex + i;
                listItems[i].SetDatum(Datas[index]);
            }
        }

        private void SelectChangedHandler(SelectChangedEventArgs args)
        {
            if (args == null || !args.IsSuccesss)
            {
                return;
            }
            GameCore.UI.Select(list, args.Items);
        }
    }
}

