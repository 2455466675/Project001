namespace GameFramework.View.UI
{
    public class UISampleData : MVC.ObservableModel
    {
        private int id;
        public int Id
        {
            get { return id; } 
            set { SetValue(ref id, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetValue(ref name, value); }
        }
    }
}


