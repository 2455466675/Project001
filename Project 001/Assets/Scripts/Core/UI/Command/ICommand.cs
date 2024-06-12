

namespace Game.UI
{
    /// <summary>
    /// 
    /// </summary>
	public interface ICommand
	{
        void Execute();
        void Undo();
	}

    public class SelectListViewCmd : ICommand
    {
        private readonly ListView listView1;
        private readonly ListView listView2;
        public SelectListViewCmd(ListView listView)
        {
            listView1 = GameCore.UI.UIRoot.ListView;
            listView2 = listView;
        }

        public void Execute()
        {
            GameCore.UI.SelectListView(listView2);
        }

        public void Undo()
        {            
            GameCore.UI.SelectListView(listView1);
        }
    }
}

