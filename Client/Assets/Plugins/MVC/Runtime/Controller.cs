namespace MVC
{
    public class Controller : IBindingManager, IObserver
    {
        private readonly BindingManager bindingManager = new BindingManager();
        private readonly Binder binder = new Binder();

        protected Binder Binder => binder;

        public Binder CreateBinder(int index)
        {
            return bindingManager.CreateBinder(index);
        }

        public Binder CreateBinder(string key)
        {
            return bindingManager.CreateBinder(key);
        }

        public void RemoveBinder(int index)
        {
            bindingManager.RemoveBinder(index);
        }

        public void RemoveBinder(string key)
        {
            bindingManager.RemoveBinder(key);
        }
        public void ClearBinding()
        {
            binder.Unbinding();
            bindingManager.ClearBinding();
        }
    }
}