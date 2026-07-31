namespace MVC
{
    public class Controller : IBindingManager, IObserver
    {
        // 默认 Binder 在 BindingManager 中占用的保留 key，业务不应使用该 key
        internal const string DefaultBinderKey = "__mvc_default_binder__";

        private readonly BindingManager bindingManager = new BindingManager();
        private Binder binder;

        protected Binder Binder => binder;

        public Controller()
        {
            // 默认 Binder 也纳入 BindingManager 统一管理，只创建一次并缓存引用
            binder = bindingManager.CreateBinderInternal(DefaultBinderKey);
        }

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
            // 统一走 BindingManager 清理所有 Binder（含默认 Binder），
            // 清理后重建默认 Binder，保证缓存引用始终由 manager 托管
            bindingManager.ClearBinding();
            binder = bindingManager.CreateBinderInternal(DefaultBinderKey);
        }
    }
}
